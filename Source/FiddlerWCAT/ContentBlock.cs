// (c)2007 Eric Lawrence
// This example is provided "AS IS" with no warranties, and confers no rights. 
//
// Prototype content-blocker allows testing of websites when some resources are blocked.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Fiddler;
using Microsoft.Win32;

public class ContentBlocker : IAutoTamper, IHandleExecAction
{
    private readonly string sSecret = new Random().Next().ToString();
    private bool bBlockerEnabled;
    private MenuItem miAutoTrim;
    private MenuItem miBlockAHost;
    private MenuItem miBlockXDomainFlash;
    private MenuItem miContentBlockEnabled;
    private MenuItem miEditBlockedHosts;
    private MenuItem miFlashAlwaysBlock;
    private MenuItem miHideBlockedSessions;
    private MenuItem miLikelyPaths;
    private MenuItem miSplit1;
    private MenuItem miSplit2;
    private MenuItem miSplit3;
    private MenuItem mnuContentBlock;
    private List<string> slBlockedHosts;

    public ContentBlocker()
    {
        // Open key with Read permissions 
        RegistryKey oReg = Registry.CurrentUser.OpenSubKey(CONFIG.GetRegPath("Root") + @"\ContentBlock\");
        if (null != oReg)
        {
            var sList = oReg.GetValue("BlockHosts", String.Empty) as String;
            if ((sList != null && sList.Length > 0))
            {
                slBlockedHosts =
                    new List<string>(sList.ToLower().Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            }
            oReg.Close();
        }

        if (null == slBlockedHosts)
        {
            slBlockedHosts = new List<string>();
        }

        InitializeMenu();
    }

    public void OnLoad()
    {
        /*
		 * NB: You might not get called here until ~after~ one of the AutoTamper methods was called.
		 * This is okay for us, because we created our mnuContentBlock in the constructor and its simply not
		 * visible anywhere until this method is called and we merge it onto the Fiddler Main menu.
		 */
        FiddlerApplication.UI.mnuMain.MenuItems.Add(mnuContentBlock);
        FiddlerApplication.UI.mnuSessionContext.MenuItems.Add(0, miBlockAHost);
    }

    public void OnBeforeUnload()
    {
        string sBlockedHosts = GetBlockedHostList();
        // Open key with Write permissions 
        RegistryKey oReg = Registry.CurrentUser.CreateSubKey(CONFIG.GetRegPath("Root") + @"\ContentBlock\");
        oReg.SetValue("BlockHosts", sBlockedHosts);
        oReg.Close();
    }

    /// <summary>
    ///     This function kills known matches early
    /// </summary>
    /// <param name="oSession"></param>
    public void AutoTamperRequestBefore(Session oSession)
    {
        // Return immediately if no rule is enabled
        if (!bBlockerEnabled) return;

        string oHost = oSession.host.ToLower();

        if ((oHost.StartsWith("ad.") ||
             oHost.StartsWith("ads.") ||
             slBlockedHosts.Contains(oHost)))
        {
            // Consider tailmatch?

            if (miHideBlockedSessions.Checked)
            {
                oSession["ui-hide"] = "userblocked";
            }
            else
            {
                oSession["ui-strikeout"] = "userblocked";
            }
            oSession["x-replywithfile"] = "1pxtrans.dat";
            return;
        }

        if (miLikelyPaths.Checked)
        {
            if (oSession.uriContains("/ad/") || oSession.uriContains("/ads/") || oSession.uriContains("/advert"))
            {
                if (!oSession.uriContains(sSecret))
                {
                    oSession.oRequest.FailSession(404, "Fiddler - ContentBlock",
                                                  "Blocked <a href='//" + oSession.url + "?&" + sSecret +
                                                  "'>Click to see</a>");
                    oSession.state = SessionStates.Done;
                    return;
                }
            }
        }

        // If Always Removing, do it and return immediately
        if (miFlashAlwaysBlock.Checked)
        {
            if ( /*oSession.url.EndsWith(".swf") ||*/ oSession.oRequest.headers.Exists("x-flash-version"))
            {
                oSession.oRequest.FailSession(404, "Fiddler - ContentBlock", "Blocked Flash");
                oSession.state = SessionStates.Done;
                return;
            }
        }
        else if (miBlockXDomainFlash.Checked)
        {
            // Issue: We don't want to block a .SWF's x-domain request for data, but we do want to block the .SWF if it's xDomain.  Hrm.
            if (oSession.uriContains(".swf")) // || oSession.oRequest.headers.Exists("x-flash-version"))
            {
                bool bBlock = false;
                string sReferer = oSession.oRequest["Referer"];

                // Allow if referer was not sent.  Note, this is a hole.
                if (sReferer == String.Empty) return;

                // Block if Referer was from another domain
                if (!bBlock)
                {
                    Uri sFromURI;
                    Uri sToURI;
                    if ((Uri.TryCreate(sReferer, UriKind.Absolute, out sFromURI)) &&
                        (Uri.TryCreate("http://" + oSession.url, UriKind.Absolute, out sToURI)))
                    {
                        bBlock = (0 !=
                                  Uri.Compare(sFromURI, sToURI, UriComponents.Host, UriFormat.Unescaped,
                                              StringComparison.InvariantCultureIgnoreCase));
                    }
                }

                if (bBlock)
                {
                    oSession.oRequest.FailSession(404, "Fiddler - ContentBlock", "Blocked Flash");
                    oSession.state = SessionStates.Done;
                }
                return;
            }
        }
    }

    public void AutoTamperRequestAfter(Session oSession)
    {
        /*noop*/
    }

    public void AutoTamperResponseBefore(Session oSession)
    {
/*noop*/
    }

    public void AutoTamperResponseAfter(Session oSession)
    {
        if (!bBlockerEnabled) return;

        if (miFlashAlwaysBlock.Checked &&
            oSession.oResponse.headers.ExistsAndContains("Content-Type", "application/x-shockwave-flash"))
        {
            oSession.responseCode = 404;
            oSession.utilSetResponseBody("Fiddler.ContentBlocked");
        }

        if (miAutoTrim.Checked && 0 == (oSession.id%10))
        {
            FiddlerApplication.UI.TrimSessionList(400);
        }
    }

    public void OnBeforeReturningError(Session oSession)
    {
/*noop*/
    }

    /// <summary>
    ///     Respond to user input from QuickExec box under the session list...
    /// </summary>
    /// <param name="sCommand"></param>
    /// <returns></returns>
    public bool OnExecAction(string sCommand)
    {
        // TODO: Add "BLOCKSITE" and "UNBLOCKSITE" commands
        if (0 == String.Compare(sCommand, "blocklist", true))
        {
            var sbBlockHosts = new StringBuilder();
            foreach (string s in slBlockedHosts)
            {
                sbBlockHosts.Append(s + "\n");
            }
            MessageBox.Show(sbBlockHosts.ToString(), "Block List...");
            return true;
        }
        return false;
    }

    private void InitializeMenu()
    {
        miBlockAHost = new MenuItem();
        miEditBlockedHosts = new MenuItem();
        mnuContentBlock = new MenuItem();
        miContentBlockEnabled = new MenuItem();
        miSplit1 = new MenuItem();
        miFlashAlwaysBlock = new MenuItem();
        miBlockXDomainFlash = new MenuItem();
        miLikelyPaths = new MenuItem();
        miAutoTrim = new MenuItem();
        miHideBlockedSessions = new MenuItem();
        miSplit2 = new MenuItem();
        miSplit3 = new MenuItem();
        // 
        // mnuContentBlock
        // 
        mnuContentBlock.MenuItems.AddRange(new[]
            {
                miContentBlockEnabled,
                miSplit1,
                miEditBlockedHosts,
                miLikelyPaths,
                miSplit2,
                miFlashAlwaysBlock,
                miBlockXDomainFlash,
                miSplit3,
                miAutoTrim,
                miHideBlockedSessions,
            });
        mnuContentBlock.Text = "&ContentBlock";

        // 
        // miContentBlockEnabled
        // 
        miContentBlockEnabled.Index = 0;
        miContentBlockEnabled.Text = "&Enabled";
        miContentBlockEnabled.Click += miBlockRule_Click;
        // 
        // miSplit1
        // 
        miSplit1.Index = 1;
        miSplit1.Text = "-";
        miSplit1.Checked = true;
        // 
        // miLikelyPaths
        // 
        miLikelyPaths.Index = 2;
        miLikelyPaths.Enabled = false;
        miLikelyPaths.Text = "&Block Paths";
        miLikelyPaths.Checked = true;
        miLikelyPaths.Click += miBlockRule_Click;
        // 
        // miEditBlockedHosts
        // 
        miEditBlockedHosts.Index = 3;
        miEditBlockedHosts.Enabled = false;
        miEditBlockedHosts.Text = "Edit B&locked Hosts...";
        miEditBlockedHosts.Click += miEditBlockedHosts_Click;
        // 
        // miSplit2
        // 
        miSplit2.Index = 4;
        miSplit2.Enabled = false;
        miSplit2.Text = "-";
        // 
        // miFlashAlwaysBlock
        // 
        miFlashAlwaysBlock.Index = 5;
        miFlashAlwaysBlock.Enabled = false;
        miFlashAlwaysBlock.Text = "Always Block &Flash";
        miFlashAlwaysBlock.Click += miBlockRule_Click;
        // 
        // miBlockXDomainFlash
        // 
        miBlockXDomainFlash.Index = 6;
        miBlockXDomainFlash.Enabled = false;
        miBlockXDomainFlash.Checked = true;
        miBlockXDomainFlash.Text = "Block &X-Domain Flash";
        miBlockXDomainFlash.Click += miBlockRule_Click;
        // 
        // miSplit3
        // 
        miSplit3.Index = 7;
        miSplit3.Text = "-";
        miSplit3.Checked = true;
        // 
        // miAutoTrim
        // 
        miAutoTrim.Index = 8;
        miAutoTrim.Enabled = false;
        miAutoTrim.Text = "&AutoTrim to 400 sessions";
        miAutoTrim.Checked = false;
        miAutoTrim.Click += miBlockRule_Click;
        // 
        // miHideBlockedSessions
        // 
        miHideBlockedSessions.Index = 9;
        miHideBlockedSessions.Enabled = false;
        miHideBlockedSessions.Text = "&Hide Blocked Sessions";
        miHideBlockedSessions.Checked = false;
        miHideBlockedSessions.Click += miBlockRule_Click;
        // 
        // miBlockAHost
        // 
        miBlockAHost.Text = "Block this Host";
        miBlockAHost.Click += miBlockAHost_Click;
    }

    public bool BlockAHost(string sHost)
    {
        if (!slBlockedHosts.Contains(sHost))
        {
            slBlockedHosts.Add(sHost);
        }
        return true;
    }

    private void miBlockAHost_Click(object sender, EventArgs e)
    {
        Session[] oSessions = FiddlerApplication.UI.GetSelectedSessions();
        foreach (Session oSession in oSessions)
        {
            try
            {
                BlockAHost(oSession.host.ToLower());
            }
            catch (Exception eX)
            {
                MessageBox.Show(eX.Message, "Cannot block host");
            }
        }
    }

    private void EnsureTransGif()
    {
        if (!File.Exists(CONFIG.GetPath("Responses") + "1pxtrans.dat"))
        {
            try
            {
                byte[] arrHeaders =
                    Encoding.ASCII.GetBytes(
                        "HTTP/1.1 200 OK\r\nContentBlock: True\r\nContent-Type: image/gif\r\nConnection: close\r\nContent-Length: 49\r\n\r\n");

                byte[] arrBody =
                    {
                        0x47, 0x49, 0x46, 0x38, 0x39, 0x61, 0x01, 0x00, 0x01, 0x00, 0x91, 0xFF, 0x00,
                        0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0xC0, 0xC0, 0xC0, 0x00, 0x00, 0x00, 0x21,
                        0xF9, 0x04, 0x01, 0x00, 0x00, 0x02, 0x00, 0x2c, 0x00, 0x00, 0x00, 0x00, 0x01,
                        0x00, 0x01, 0x00, 0x00, 0x02, 0x02, 0x54, 0x01, 0x00, 0x3B
                    };

                FileStream oFS = File.Create(CONFIG.GetPath("Responses") + "1pxtrans.dat");
                oFS.Write(arrHeaders, 0, arrHeaders.Length);
                oFS.Write(arrBody, 0, arrBody.Length);
                oFS.Close();
            }
            catch (Exception eX)
            {
                MessageBox.Show(eX.ToString(), "Failed to create transparent gif...");
            }
        }
    }

    private void miEditBlockedHosts_Click(object sender, EventArgs e)
    {
        string sNewList = frmPrompt.GetUserString("Edit Blocked Host List", "Enter semi-colon delimited block list.",
                                                  GetBlockedHostList(), true);
        if (null == sNewList)
        {
            FiddlerApplication.UI.sbpInfo.Text = "Block list left unchanged.";
            return;
        }
        else
        {
            FiddlerApplication.UI.sbpInfo.Text = "Block list updated.";
            slBlockedHosts =
                new List<string>(sNewList.ToLower().Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
        }
    }

    /// <summary>
    ///     The logic for unchecking dependent options in this menu system isn't quite right, but it's fine for now.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void miBlockRule_Click(object sender, EventArgs e)
    {
        var oSender = (sender as MenuItem);
        oSender.Checked = !oSender.Checked;

        bBlockerEnabled = miContentBlockEnabled.Checked;
        if (bBlockerEnabled)
        {
            EnsureTransGif();
        }

        // Enable menuitems based on overall enabled state.
        miEditBlockedHosts.Enabled =
            miFlashAlwaysBlock.Enabled = miAutoTrim.Enabled = miLikelyPaths.Enabled = miHideBlockedSessions.Enabled =
                                                                                      miBlockXDomainFlash.Enabled =
                                                                                      miSplit2.Enabled = bBlockerEnabled;
    }

    private string GetBlockedHostList()
    {
        var sbBlockHosts = new StringBuilder();
        foreach (string s in slBlockedHosts)
        {
            if (s.Trim().Length > 3)
            {
                sbBlockHosts.Append(s.Trim() + ";");
            }
        }
        return sbBlockHosts.ToString();
    }
}
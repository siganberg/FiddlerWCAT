using System.Windows.Forms;

namespace FiddlerWCAT
{
	using Fiddler;
	public class CapacityTestingTool : IAutoTamper    // Ensure class is public, or Fiddler won't see it!
	{
		string sUserAgent = "";
		private MenuItem wcatMenu;
		private MenuItem wcatConfig;

		public void InitializeMenu()
		{
			wcatMenu = new MenuItem();
			wcatMenu.Text = "&WCAT Tools";

			wcatConfig = new MenuItem();
			wcatConfig.Text = "Configuration";
		    wcatConfig.Enabled = true;
            wcatConfig.Click += wcatConfig_Click;
               
			wcatMenu.MenuItems.AddRange(new[] { wcatConfig });
		}

        void wcatConfig_Click(object sender, System.EventArgs e)
        {
            var settingForm = new SettingsForm();
            settingForm.ShowDialog();
        }


		public CapacityTestingTool()
		{
			InitializeMenu();
		}

		public void OnLoad()
		{
			FiddlerApplication.UI.mnuMain.MenuItems.Add(wcatMenu);
		}
		public void OnBeforeUnload() { }

		public void AutoTamperRequestBefore(Session oSession)
		{
		}

		public void AutoTamperRequestAfter(Session oSession) { }
		public void AutoTamperResponseBefore(Session oSession) { }
		public void AutoTamperResponseAfter(Session oSession) { }
		public void OnBeforeReturningError(Session oSession) { }
	}
}

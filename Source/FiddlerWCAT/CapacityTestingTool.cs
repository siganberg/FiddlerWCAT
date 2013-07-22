// <copyright file=" " company="Digitrish">
// Copyright (c) 2013 All Right Reserved, http://www.digitrish.com
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
// </copyright>
//
// <author>Francis Marasigan</author>
// <email>francis.marasigan@live.com</email>
// <date>2013-06-23</date>

using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using Fiddler;
using System.Threading;
using FiddlerWCAT.Forms;

namespace FiddlerWCAT
{

    public class CapacityTestingTool : IAutoTamper    // Ensure class is public, or Fiddler won't see it!
    {
        private MenuItem _wcatMenu;
        private MenuItem _wcatConfig;
        private MenuItem _wcatSingleScenario;
        private MenuItem _wcatMultipleScenario;


        public void InitializeMenu()
        {
            _wcatMenu = new MenuItem { Text = "&WCAT Tools" };

            _wcatConfig = new MenuItem { Text = "Configuration", Enabled = true };
            _wcatConfig.Click += wcatConfig_Click;

            _wcatMenu.MenuItems.AddRange(new[] { _wcatConfig });

            _wcatSingleScenario = new MenuItem { Text = "Run WCAT one scenario for all selected sessions." };
            _wcatSingleScenario.Click += wcatSingleScenario_Click;

            _wcatMultipleScenario = new MenuItem { Text = "Run WCAT one scenario per selected sessions." };
            _wcatMultipleScenario.Click += _wcatMultipleScenario_Click;
        }

        void _wcatMultipleScenario_Click(object sender, System.EventArgs e)
        {
            var oSessions = FiddlerApplication.UI.GetSelectedSessions();
            ThreadPool.QueueUserWorkItem(delegate { 
                foreach(var oSession in oSessions)
                {
                    var scenario = CreateScenario(new [] { oSession });
                    var result = scenario.Run();
                    if (result != 0) break;
                    Thread.Sleep(1000); 
                }
            });
        }

        private Scenario CreateScenario(IEnumerable<Session> oSessions)
        {
            var scenario = new Scenario();

            var def = new Default();
            scenario.Duration = Settings.Instance.Duration;
            scenario.Cooldown = Settings.Instance.Cooldown;
            scenario.Warmup = Settings.Instance.Warmup;
            scenario.ThrottleRps = Settings.Instance.ThrottleRps;
            scenario.Default = def;

            foreach (var oSession in oSessions)
            {
                var transaction = new Transaction { Id = oSession.id.ToString(CultureInfo.InvariantCulture), Weight = 1 };

                var request = new Request { Url = oSession.PathAndQuery, Server = oSession.hostname };
                foreach (var h in oSession.oRequest.headers)
                {
                    var header = new Header { Name = h.Name, Value = h.Value };
                    request.SetHeader.Add(header);

                    var postData = oSession.GetRequestBodyAsString();
                    if (!string.IsNullOrEmpty(postData))
                    {
                        request.Verb = Verb.Post;
                        request.PostData = postData;
                    }
                }
                transaction.Request.Add(request);
                scenario.Transaction.Add(transaction);
            }

            scenario.Optimize();
            return scenario;
        }


        void wcatSingleScenario_Click(object sender, System.EventArgs e)
        {
            var oSessions = FiddlerApplication.UI.GetSelectedSessions();
            var scenario = CreateScenario(oSessions);
            scenario.Run();
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
            FiddlerApplication.UI.mnuMain.MenuItems.Add(_wcatMenu);
            FiddlerApplication.UI.mnuSessionContext.MenuItems.Add(_wcatSingleScenario);
            FiddlerApplication.UI.mnuSessionContext.MenuItems.Add(_wcatMultipleScenario);

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

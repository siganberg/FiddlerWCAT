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
	  
using System.Globalization;
using System.Windows.Forms;
using Fiddler;
using FiddlerWCAT.Entities;
using System.Threading;

namespace FiddlerWCAT
{

	public class CapacityTestingTool : IAutoTamper    // Ensure class is public, or Fiddler won't see it!
	{
		private MenuItem _wcatMenu;
		private MenuItem _wcatConfig;
		private MenuItem _wcatRunSelection;
		private MenuItem _wcatRunSelectionOnce;


		public void InitializeMenu()
		{
			_wcatMenu = new MenuItem {Text = "&WCAT Tools"};

			_wcatConfig = new MenuItem {Text = "Configuration", Enabled = true};
			_wcatConfig.Click += wcatConfig_Click;
			   
			_wcatMenu.MenuItems.AddRange(new[] { _wcatConfig });

			_wcatRunSelection = new MenuItem {Text = "Run WCAT on selected sesions"};
			_wcatRunSelection.Click += wcatRunSelection_Click;

			_wcatRunSelectionOnce = new MenuItem { Text = "Run WCAT on per selected sessions" };
			_wcatRunSelectionOnce.Click += _wcatRunSelectionOnce_Click;
		}

		void _wcatRunSelectionOnce_Click(object sender, System.EventArgs e)
		{
			var oSessions = FiddlerApplication.UI.GetSelectedSessions();
			System.Threading.ThreadPool.QueueUserWorkItem(delegate
			{
				RunIndividualSession(oSessions);
			}, null);
		}

		private void RunIndividualSession(Session[] sessions)
		{


			for (int index = 0; index < sessions.Length; index++)
			{
				var oSession = sessions[index];

				var scenario = new Scenario();
				var def = new Default();
				scenario.Name = index.ToString(CultureInfo.InvariantCulture);
				scenario.Duration = Settings.Instance.Duration;
				scenario.Cooldown = Settings.Instance.Cooldown;
				scenario.Warmup = Settings.Instance.Warmup;
				scenario.ThrottleRps = Settings.Instance.ThrottleRps;
				scenario.Default = def;

				var transaction = new Transaction {Id = oSession.id.ToString(CultureInfo.InvariantCulture), Weight = 1};

				var request = new Request {Url = oSession.PathAndQuery, Server = oSession.hostname};
				foreach (var h in oSession.oRequest.headers)
				{
					var header = new Header {Name = h.Name, Value = h.Value};
					request.SetHeader.Add(header);

					var postData = oSession.GetRequestBodyAsString();
					if (!string.IsNullOrEmpty(postData))
					{
						request.Verb = Verb.POST;
						request.PostData = postData;
					}
				}
				transaction.Request.Add(request);
				scenario.Transaction.Add(transaction);

				scenario.Optimize();
				scenario.Save();
				var result = scenario.Run();

				if (result != 0) break; 

                Thread.Sleep(1000); //-- give some time before the next scenario.
			}


		}



		void wcatRunSelection_Click(object sender, System.EventArgs e)
		{


			var oSessions = FiddlerApplication.UI.GetSelectedSessions();
			
			//-- Create scenario 
			var scenario = new Scenario();
	  
			var def = new Default();
			scenario.Duration = Settings.Instance.Duration;
			scenario.Cooldown = Settings.Instance.Cooldown;
			scenario.Warmup = Settings.Instance.Warmup;
			scenario.ThrottleRps = Settings.Instance.ThrottleRps; 
			scenario.Default = def; 
			

			foreach (var oSession in oSessions)
			{
				var transaction = new Transaction {Id = oSession.id.ToString(CultureInfo.InvariantCulture), Weight = 1};

				var request = new Request {Url = oSession.PathAndQuery, Server = oSession.hostname};
				foreach (var h in oSession.oRequest.headers)
				{
					var header = new Header {Name = h.Name, Value = h.Value};
					request.SetHeader.Add(header);

					var postData = oSession.GetRequestBodyAsString();
					if (!string.IsNullOrEmpty(postData))
					{
						request.Verb = Verb.POST;
						request.PostData = postData;
					}
				}
				transaction.Request.Add(request);
				scenario.Transaction.Add(transaction);
			}

			scenario.Optimize();
			scenario.Save();
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
			FiddlerApplication.UI.mnuSessionContext.MenuItems.Add(_wcatRunSelection);
			FiddlerApplication.UI.mnuSessionContext.MenuItems.Add(_wcatRunSelectionOnce);

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

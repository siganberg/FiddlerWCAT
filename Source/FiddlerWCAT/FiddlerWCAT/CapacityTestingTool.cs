using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Fiddler;
using FiddlerWCAT.Entities;

namespace FiddlerWCAT
{

	public class CapacityTestingTool : IAutoTamper    // Ensure class is public, or Fiddler won't see it!
	{
		private MenuItem _wcatMenu;
		private MenuItem _wcatConfig;
		private MenuItem _wcatRunSelection;

		public void InitializeMenu()
		{
			_wcatMenu = new MenuItem {Text = "&WCAT Tools"};

			_wcatConfig = new MenuItem {Text = "Configuration", Enabled = true};
			_wcatConfig.Click += wcatConfig_Click;
			   
			_wcatMenu.MenuItems.AddRange(new[] { _wcatConfig });

			_wcatRunSelection = new MenuItem {Text = "Run WCAT On Selected Item"};
			_wcatRunSelection.Click += wcatRunSelection_Click;
		}

		void wcatRunSelection_Click(object sender, System.EventArgs e)
		{

			//-- wcat.wsf -terminate -run -clients localhost,dmgdevv12 -t invalid_header.ubr -s eagl.spe.sony.com -v %1 
			var oSessions = FiddlerApplication.UI.GetSelectedSessions();
			
			//-- Create scenario 
			var scenario = new Scenario();
	  
			var def = new Default();
			scenario.Default = def; 

			foreach (var oSession in oSessions)
			{
				var transaction = new Transaction {Id = oSession.id.ToString(CultureInfo.InvariantCulture), Weight = 1};

				var request = new Request {Url = oSession.PathAndQuery, Server = oSession.hostname};
				foreach (var h in oSession.oRequest.headers)
				{
					var header = new Header {Name = h.Name, Value = h.Value};
					request.SetHeader.Add(header);
				}
				transaction.Request.Add(request);
				scenario.Transaction.Add(transaction);
			}

			var fs = new FileStream(@"c:\test.ubr", FileMode.Create);
			var formatter = new CFormatter();
			formatter.Serialize(fs, scenario);
			fs.Close();
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

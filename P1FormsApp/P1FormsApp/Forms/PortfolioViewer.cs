using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using P1_SECCL_API.Classes;
using Services.DataHandling;

namespace P1FormsApp
{
    public partial class PortfolioViewer : Form
    {
        //TODO: Import the API and Middleware Services
        private readonly IMiddleware _middleware;

        private const string _firmID = "P1IMX";
        private const string _userID = "nelahi6642@4tmail.net";
        private const string _password = "DemoBDM1";

        private Authentication.AuthenticationToken _authToken;

        // Keeping a global list of accounts, so that we can do some on selected item stuff.
        private List<Portfolio.PorfolioAccount> _accounts;
        
        private ListView lvPortfolios;
        private GroupBox gbSelected;
        private GroupBox gbSummary;
        private TextBox txtSurname;
        private Label lblSurname;
        private TextBox txtFirstName;
        private Label lblFirstName;
        private Label lblClientType;
        private TextBox txtClientType;
        private TextBox txtCurrency;
        private Label lblCurrency;
        private TextBox txtLanguage;
        private Label lblLanguage;
        private TextBox txtStatus;
        private Label lblStatus;
        private TextBox txtPortfolioCount;
        private Label lblPortfolioCount;
        private Label lblFirmAverage;
        private TextBox txtAverageValue;
        private TextBox txtFirmID;
        private Label label1;
        private TextBox txtID;
        private Label lblID;
        private GroupBox gbAccounts;
        private GroupBox gbPositions;
        private ListView lvAccounts;
        private Label lblPositionTotal;
        private TextBox txtTotalAccountsValue;
        private Label lblTotalAccountsValue;
        private TextBox txtTotalPositionValue;
        private ListView lvPositions;
        private int _selectedPortfolioIndex = -1;

        public PortfolioViewer(IMiddleware _Middleware)
        {
            // Assigning our middleware so that we can make calls to get data.
            _middleware = _Middleware;

            InitializeComponent();
            // Once all of the components have been initialized, we can start to set up the event listeners we want.
            InitializeListners();

            // Now we want to Authenicate our session, so that we can make other API calls.
            _authToken = _middleware.AuthenticateSession(_firmID, _userID, _password);    
            
            // Loading the list of Portfolios to be displayed to the user.
            LoadPorfolios();

            // Based off of the Portfolios we have just loaded, we want to bind the overall summary for the account.
            BindFirmSummary();
        }

        /// <summary>
        /// Initializing any event listeners that we want to Form components.
        /// </summary>
        private void InitializeListners()
        {
            lvPortfolios.ItemSelectionChanged += LvPortfolios_ItemSelectionChanged;
        }

        /// <summary>
        /// On Click event handler for when an item is clicked in the List View of Portfolios.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LvPortfolios_ItemSelectionChanged(object? sender, ListViewItemSelectionChangedEventArgs e)
        {
            // If the new selected item is the same as the previous selected index, we do not want to continue with the method.
            if(e.ItemIndex == _selectedPortfolioIndex)
            {
                return;
            }

            // Update selected index
            _selectedPortfolioIndex = e.ItemIndex;

            // Getting the portfolio at the index we have selected from the accounts stored in memory. We dont want to have to call APIs unless we really need to.
            Portfolio.PorfolioAccount selectedAccount = _accounts[_selectedPortfolioIndex];

            // Updating the information for the selected account.
            Portfolio.PortfolioSummary summary = _middleware.GetPortfoliosSummary(_firmID, selectedAccount.ID, _authToken.Token);

            txtID.Text = summary.ID;
            txtFirmID.Text = summary.FirmID;

            txtFirstName.Text = summary.FirstName;
            txtSurname.Text = summary.Surname;

            txtClientType.Text = summary.ClientType;

            txtLanguage.Text = summary.Language;
            txtCurrency.Text = summary.Currency;
            txtStatus.Text = summary.Status;            

            BindPositions(summary);
            BindAccounts(summary);
        }

        /// <summary>
        /// Binds the "Positions" section of the GUI
        /// </summary>
        /// <param name="_Portfolio"></param>
        private void BindPositions(Portfolio.PortfolioSummary _Portfolio)
        {
            // Clearing old data from List View
            lvPositions.Items.Clear();

            // Setting the total TB
            txtTotalPositionValue.Text = _middleware.GetTotalPositionValue(_Portfolio).ToString("0,0.00");

            // Iterating over all of the Positions
            foreach(Portfolio.PortfolioPosition pos in _Portfolio.Positions)
            {
                string listViewDisplay = "";

                // If the Position does not have an asset name, we will jsut display it like this: "CASH - 0.00"
                if (string.IsNullOrWhiteSpace(pos.AssetName))
                {
                    listViewDisplay = $"{pos.PositionType} - {pos.CurrentValue.ToString("0,0.00")}";
                }
                else
                {
                    // Otherwise, we will display it like this: "TYPE - NAME - 0.00"
                    listViewDisplay = $"{pos.PositionType} - {pos.AssetName} - {pos.CurrentValue.ToString("0,0.00")}";
                }

                lvPositions.Items.Add(listViewDisplay);
            }
        }

        /// <summary>
        /// Binding the "Account" section of the GUI
        /// </summary>
        /// <param name="_Portfolio"></param>
        private void BindAccounts(Portfolio.PortfolioSummary _Portfolio)
        {
            // Clearing out the List View
            lvAccounts.Items.Clear();

            // Setting the total
            txtTotalAccountsValue.Text = _middleware.GetTotalAccountsValue(_Portfolio).ToString("0,0.00");

            // Iterating over and creating a new List View Item to display text like: "ACCOUNT NAME - TYPE - 0.00"
            foreach (Portfolio.SubAccount acc in _Portfolio.Accounts)
            {
                string listViewDisplay = "";

                listViewDisplay = $"{acc.Name} - {acc.WrapperType} - {acc.CurrentValue.ToString("0,0.00")}";

                lvAccounts.Items.Add(listViewDisplay);
            }

        }

        /// <summary>
        /// Form Event called when this Form is successfully loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PortfolioViewer_Load(object sender, EventArgs e)
        {
            LoadPorfolios();

            BindFirmSummary();
        }

        /// <summary>
        /// Loads the list of Porfolios for the Firm ID.
        /// </summary>
        private void LoadPorfolios()
        {
            // Clearing the items from the list view, so that we dont have duplicated data.
            lvPortfolios.Items.Clear();

            // On load, we want to call the middleware to get the list of portfolios and bind them to the list view.
            _accounts = _middleware.GetPortfoliosForFirm(_firmID, _authToken.Token);

            // Iterating over all of the accounts and adding them to the list view to be displayed.
            foreach (Portfolio.PorfolioAccount account in _accounts)
            {
                lvPortfolios.Items.Add(new ListViewItem($"{ account.ID } - { account.Name }"));
            }
        }

        /// <summary>
        /// Bind the Summary section of the form for the Firm ID.
        /// </summary>
        private void BindFirmSummary()
        {
            txtPortfolioCount.Text = _accounts.Count.ToString();

            decimal average = _middleware.GetFirmAverageCashValue(_accounts);
            txtAverageValue.Text = $"{average.ToString("0,0.00")}";

        }

        /// <summary>
        /// Generic Form method which initializes all of the items we have added to the form.
        /// </summary>
        private void InitializeComponent()
        {
            this.lvPortfolios = new System.Windows.Forms.ListView();
            this.gbSelected = new System.Windows.Forms.GroupBox();
            this.gbAccounts = new System.Windows.Forms.GroupBox();
            this.txtTotalAccountsValue = new System.Windows.Forms.TextBox();
            this.lblTotalAccountsValue = new System.Windows.Forms.Label();
            this.lvAccounts = new System.Windows.Forms.ListView();
            this.gbPositions = new System.Windows.Forms.GroupBox();
            this.txtTotalPositionValue = new System.Windows.Forms.TextBox();
            this.lblPositionTotal = new System.Windows.Forms.Label();
            this.txtFirmID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtID = new System.Windows.Forms.TextBox();
            this.lblID = new System.Windows.Forms.Label();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtCurrency = new System.Windows.Forms.TextBox();
            this.lblCurrency = new System.Windows.Forms.Label();
            this.txtLanguage = new System.Windows.Forms.TextBox();
            this.lblLanguage = new System.Windows.Forms.Label();
            this.txtClientType = new System.Windows.Forms.TextBox();
            this.lblClientType = new System.Windows.Forms.Label();
            this.txtSurname = new System.Windows.Forms.TextBox();
            this.lblSurname = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.gbSummary = new System.Windows.Forms.GroupBox();
            this.txtPortfolioCount = new System.Windows.Forms.TextBox();
            this.lblPortfolioCount = new System.Windows.Forms.Label();
            this.lblFirmAverage = new System.Windows.Forms.Label();
            this.txtAverageValue = new System.Windows.Forms.TextBox();
            this.lvPositions = new System.Windows.Forms.ListView();
            this.gbSelected.SuspendLayout();
            this.gbAccounts.SuspendLayout();
            this.gbPositions.SuspendLayout();
            this.gbSummary.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvPortfolios
            // 
            this.lvPortfolios.Location = new System.Drawing.Point(12, 12);
            this.lvPortfolios.MultiSelect = false;
            this.lvPortfolios.Name = "lvPortfolios";
            this.lvPortfolios.Size = new System.Drawing.Size(276, 462);
            this.lvPortfolios.TabIndex = 0;
            this.lvPortfolios.UseCompatibleStateImageBehavior = false;
            this.lvPortfolios.View = System.Windows.Forms.View.List;
            // 
            // gbSelected
            // 
            this.gbSelected.Controls.Add(this.gbAccounts);
            this.gbSelected.Controls.Add(this.gbPositions);
            this.gbSelected.Controls.Add(this.txtFirmID);
            this.gbSelected.Controls.Add(this.label1);
            this.gbSelected.Controls.Add(this.txtID);
            this.gbSelected.Controls.Add(this.lblID);
            this.gbSelected.Controls.Add(this.txtStatus);
            this.gbSelected.Controls.Add(this.lblStatus);
            this.gbSelected.Controls.Add(this.txtCurrency);
            this.gbSelected.Controls.Add(this.lblCurrency);
            this.gbSelected.Controls.Add(this.txtLanguage);
            this.gbSelected.Controls.Add(this.lblLanguage);
            this.gbSelected.Controls.Add(this.txtClientType);
            this.gbSelected.Controls.Add(this.lblClientType);
            this.gbSelected.Controls.Add(this.txtSurname);
            this.gbSelected.Controls.Add(this.lblSurname);
            this.gbSelected.Controls.Add(this.txtFirstName);
            this.gbSelected.Controls.Add(this.lblFirstName);
            this.gbSelected.Location = new System.Drawing.Point(294, 12);
            this.gbSelected.Name = "gbSelected";
            this.gbSelected.Size = new System.Drawing.Size(427, 336);
            this.gbSelected.TabIndex = 1;
            this.gbSelected.TabStop = false;
            this.gbSelected.Text = "Selected Portfolio";
            // 
            // gbAccounts
            // 
            this.gbAccounts.Controls.Add(this.txtTotalAccountsValue);
            this.gbAccounts.Controls.Add(this.lblTotalAccountsValue);
            this.gbAccounts.Controls.Add(this.lvAccounts);
            this.gbAccounts.Location = new System.Drawing.Point(215, 162);
            this.gbAccounts.Name = "gbAccounts";
            this.gbAccounts.Size = new System.Drawing.Size(206, 168);
            this.gbAccounts.TabIndex = 17;
            this.gbAccounts.TabStop = false;
            this.gbAccounts.Text = "Accounts";
            // 
            // txtTotalAccountsValue
            // 
            this.txtTotalAccountsValue.Enabled = false;
            this.txtTotalAccountsValue.Location = new System.Drawing.Point(121, 16);
            this.txtTotalAccountsValue.Name = "txtTotalAccountsValue";
            this.txtTotalAccountsValue.Size = new System.Drawing.Size(76, 23);
            this.txtTotalAccountsValue.TabIndex = 4;
            // 
            // lblTotalAccountsValue
            // 
            this.lblTotalAccountsValue.AutoSize = true;
            this.lblTotalAccountsValue.Location = new System.Drawing.Point(3, 19);
            this.lblTotalAccountsValue.Name = "lblTotalAccountsValue";
            this.lblTotalAccountsValue.Size = new System.Drawing.Size(114, 15);
            this.lblTotalAccountsValue.TabIndex = 3;
            this.lblTotalAccountsValue.Text = "Total Account Value:";
            // 
            // lvAccounts
            // 
            this.lvAccounts.Location = new System.Drawing.Point(6, 45);
            this.lvAccounts.Name = "lvAccounts";
            this.lvAccounts.Size = new System.Drawing.Size(192, 117);
            this.lvAccounts.TabIndex = 0;
            this.lvAccounts.UseCompatibleStateImageBehavior = false;
            this.lvAccounts.View = System.Windows.Forms.View.List;
            // 
            // gbPositions
            // 
            this.gbPositions.Controls.Add(this.lvPositions);
            this.gbPositions.Controls.Add(this.txtTotalPositionValue);
            this.gbPositions.Controls.Add(this.lblPositionTotal);
            this.gbPositions.Location = new System.Drawing.Point(6, 162);
            this.gbPositions.Name = "gbPositions";
            this.gbPositions.Size = new System.Drawing.Size(206, 168);
            this.gbPositions.TabIndex = 16;
            this.gbPositions.TabStop = false;
            this.gbPositions.Text = "Positions";
            // 
            // txtTotalPositionValue
            // 
            this.txtTotalPositionValue.Enabled = false;
            this.txtTotalPositionValue.Location = new System.Drawing.Point(124, 16);
            this.txtTotalPositionValue.Name = "txtTotalPositionValue";
            this.txtTotalPositionValue.Size = new System.Drawing.Size(76, 23);
            this.txtTotalPositionValue.TabIndex = 2;
            // 
            // lblPositionTotal
            // 
            this.lblPositionTotal.AutoSize = true;
            this.lblPositionTotal.Location = new System.Drawing.Point(6, 19);
            this.lblPositionTotal.Name = "lblPositionTotal";
            this.lblPositionTotal.Size = new System.Drawing.Size(112, 15);
            this.lblPositionTotal.TabIndex = 1;
            this.lblPositionTotal.Text = "Total Position Value:";
            // 
            // txtFirmID
            // 
            this.txtFirmID.Enabled = false;
            this.txtFirmID.Location = new System.Drawing.Point(112, 37);
            this.txtFirmID.Name = "txtFirmID";
            this.txtFirmID.Size = new System.Drawing.Size(100, 23);
            this.txtFirmID.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(112, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 15);
            this.label1.TabIndex = 14;
            this.label1.Text = "Firm ID:";
            // 
            // txtID
            // 
            this.txtID.Enabled = false;
            this.txtID.Location = new System.Drawing.Point(6, 37);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(100, 23);
            this.txtID.TabIndex = 13;
            // 
            // lblID
            // 
            this.lblID.AutoSize = true;
            this.lblID.Location = new System.Drawing.Point(6, 19);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(21, 15);
            this.lblID.TabIndex = 12;
            this.lblID.Text = "ID:";
            // 
            // txtStatus
            // 
            this.txtStatus.Enabled = false;
            this.txtStatus.Location = new System.Drawing.Point(313, 37);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(100, 23);
            this.txtStatus.TabIndex = 11;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(313, 19);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(42, 15);
            this.lblStatus.TabIndex = 10;
            this.lblStatus.Text = "Status:";
            // 
            // txtCurrency
            // 
            this.txtCurrency.Enabled = false;
            this.txtCurrency.Location = new System.Drawing.Point(74, 133);
            this.txtCurrency.Name = "txtCurrency";
            this.txtCurrency.Size = new System.Drawing.Size(58, 23);
            this.txtCurrency.TabIndex = 9;
            // 
            // lblCurrency
            // 
            this.lblCurrency.AutoSize = true;
            this.lblCurrency.Location = new System.Drawing.Point(74, 115);
            this.lblCurrency.Name = "lblCurrency";
            this.lblCurrency.Size = new System.Drawing.Size(58, 15);
            this.lblCurrency.TabIndex = 8;
            this.lblCurrency.Text = "Currency:";
            // 
            // txtLanguage
            // 
            this.txtLanguage.Enabled = false;
            this.txtLanguage.Location = new System.Drawing.Point(6, 133);
            this.txtLanguage.Name = "txtLanguage";
            this.txtLanguage.Size = new System.Drawing.Size(62, 23);
            this.txtLanguage.TabIndex = 7;
            // 
            // lblLanguage
            // 
            this.lblLanguage.AutoSize = true;
            this.lblLanguage.Location = new System.Drawing.Point(6, 115);
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.Size = new System.Drawing.Size(62, 15);
            this.lblLanguage.TabIndex = 6;
            this.lblLanguage.Text = "Language:";
            // 
            // txtClientType
            // 
            this.txtClientType.Enabled = false;
            this.txtClientType.Location = new System.Drawing.Point(313, 84);
            this.txtClientType.Name = "txtClientType";
            this.txtClientType.Size = new System.Drawing.Size(100, 23);
            this.txtClientType.TabIndex = 5;
            // 
            // lblClientType
            // 
            this.lblClientType.AutoSize = true;
            this.lblClientType.Location = new System.Drawing.Point(313, 66);
            this.lblClientType.Name = "lblClientType";
            this.lblClientType.Size = new System.Drawing.Size(68, 15);
            this.lblClientType.TabIndex = 4;
            this.lblClientType.Text = "Client Type:";
            // 
            // txtSurname
            // 
            this.txtSurname.Enabled = false;
            this.txtSurname.Location = new System.Drawing.Point(112, 84);
            this.txtSurname.Name = "txtSurname";
            this.txtSurname.Size = new System.Drawing.Size(100, 23);
            this.txtSurname.TabIndex = 3;
            // 
            // lblSurname
            // 
            this.lblSurname.AutoSize = true;
            this.lblSurname.Location = new System.Drawing.Point(112, 66);
            this.lblSurname.Name = "lblSurname";
            this.lblSurname.Size = new System.Drawing.Size(57, 15);
            this.lblSurname.TabIndex = 2;
            this.lblSurname.Text = "Surname:";
            // 
            // txtFirstName
            // 
            this.txtFirstName.Enabled = false;
            this.txtFirstName.Location = new System.Drawing.Point(6, 84);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(100, 23);
            this.txtFirstName.TabIndex = 1;
            // 
            // lblFirstName
            // 
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Location = new System.Drawing.Point(6, 66);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(67, 15);
            this.lblFirstName.TabIndex = 0;
            this.lblFirstName.Text = "First Name:";
            // 
            // gbSummary
            // 
            this.gbSummary.Controls.Add(this.txtPortfolioCount);
            this.gbSummary.Controls.Add(this.lblPortfolioCount);
            this.gbSummary.Controls.Add(this.lblFirmAverage);
            this.gbSummary.Controls.Add(this.txtAverageValue);
            this.gbSummary.Location = new System.Drawing.Point(294, 354);
            this.gbSummary.Name = "gbSummary";
            this.gbSummary.Size = new System.Drawing.Size(427, 120);
            this.gbSummary.TabIndex = 2;
            this.gbSummary.TabStop = false;
            this.gbSummary.Text = "Firm Summary";
            // 
            // txtPortfolioCount
            // 
            this.txtPortfolioCount.Enabled = false;
            this.txtPortfolioCount.Location = new System.Drawing.Point(6, 37);
            this.txtPortfolioCount.Name = "txtPortfolioCount";
            this.txtPortfolioCount.Size = new System.Drawing.Size(100, 23);
            this.txtPortfolioCount.TabIndex = 3;
            // 
            // lblPortfolioCount
            // 
            this.lblPortfolioCount.AutoSize = true;
            this.lblPortfolioCount.Location = new System.Drawing.Point(6, 19);
            this.lblPortfolioCount.Name = "lblPortfolioCount";
            this.lblPortfolioCount.Size = new System.Drawing.Size(92, 15);
            this.lblPortfolioCount.TabIndex = 2;
            this.lblPortfolioCount.Text = "Portfolio Count:";
            // 
            // lblFirmAverage
            // 
            this.lblFirmAverage.AutoSize = true;
            this.lblFirmAverage.Location = new System.Drawing.Point(6, 73);
            this.lblFirmAverage.Name = "lblFirmAverage";
            this.lblFirmAverage.Size = new System.Drawing.Size(113, 15);
            this.lblFirmAverage.TabIndex = 1;
            this.lblFirmAverage.Text = "Average Cash Value:";
            // 
            // txtAverageValue
            // 
            this.txtAverageValue.Enabled = false;
            this.txtAverageValue.Location = new System.Drawing.Point(6, 91);
            this.txtAverageValue.Name = "txtAverageValue";
            this.txtAverageValue.Size = new System.Drawing.Size(100, 23);
            this.txtAverageValue.TabIndex = 0;
            // 
            // lvPositions
            // 
            this.lvPositions.Location = new System.Drawing.Point(6, 45);
            this.lvPositions.Name = "lvPositions";
            this.lvPositions.Size = new System.Drawing.Size(194, 117);
            this.lvPositions.TabIndex = 3;
            this.lvPositions.UseCompatibleStateImageBehavior = false;
            this.lvPositions.View = System.Windows.Forms.View.List;
            // 
            // PortfolioViewer
            // 
            this.ClientSize = new System.Drawing.Size(733, 486);
            this.Controls.Add(this.gbSummary);
            this.Controls.Add(this.gbSelected);
            this.Controls.Add(this.lvPortfolios);
            this.Name = "PortfolioViewer";
            this.gbSelected.ResumeLayout(false);
            this.gbSelected.PerformLayout();
            this.gbAccounts.ResumeLayout(false);
            this.gbAccounts.PerformLayout();
            this.gbPositions.ResumeLayout(false);
            this.gbPositions.PerformLayout();
            this.gbSummary.ResumeLayout(false);
            this.gbSummary.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}

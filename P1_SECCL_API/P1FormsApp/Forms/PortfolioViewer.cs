﻿using System;
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
        private int _selectedPortfolioIndex = 0;

        public PortfolioViewer(IMiddleware _Middleware)
        {
            _middleware = _Middleware;

            InitializeComponent();
            InitializeListners();

            _authToken = _middleware.AuthenticateSession(_firmID, _userID, _password);

            // On load, we want to call the middleware to get the list of portfolios and bind them to the list view.
            _accounts = _middleware.GetPortfoliosForFirm(_firmID, _authToken.Token);

            // Iterating over all of the accounts and adding them to the list view to be displayed.
            foreach (Portfolio.PorfolioAccount account in _accounts)
            {
                lvPortfolios.Items.Add(account.Name);
            }
        }

        private void InitializeListners()
        {
            lvPortfolios.ItemSelectionChanged += LvPortfolios_ItemSelectionChanged;
        }

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

            // TODO: Updating the information for the selected account.
            Portfolio.PortfolioSummary summary = _middleware.GetPortfoliosSummary(_firmID, selectedAccount.ID, _authToken.Token);

            txtFirstName.Text = summary.FirstName;
            txtSurname.Text = summary.Surname;
        }

        private void PortfolioViewer_Load(object sender, EventArgs e)
        {
            // On load, we want to call the middleware to get the list of portfolios and bind them to the list view.
            _accounts = _middleware.GetPortfoliosForFirm(_firmID, _authToken.Token);

            // Iterating over all of the accounts and adding them to the list view to be displayed.
            foreach(Portfolio.PorfolioAccount account in _accounts)
            {
                lvPortfolios.Items.Add(account.Name);
            }
        }

        private void InitializeComponent()
        {
            this.lvPortfolios = new System.Windows.Forms.ListView();
            this.gbSelected = new System.Windows.Forms.GroupBox();
            this.txtSurname = new System.Windows.Forms.TextBox();
            this.lblSurname = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.gbSummary = new System.Windows.Forms.GroupBox();
            this.gbSelected.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvPortfolios
            // 
            this.lvPortfolios.Location = new System.Drawing.Point(12, 12);
            this.lvPortfolios.Name = "lvPortfolios";
            this.lvPortfolios.Size = new System.Drawing.Size(276, 462);
            this.lvPortfolios.TabIndex = 0;
            this.lvPortfolios.UseCompatibleStateImageBehavior = false;
            // 
            // gbSelected
            // 
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
            // txtSurname
            // 
            this.txtSurname.Enabled = false;
            this.txtSurname.Location = new System.Drawing.Point(112, 37);
            this.txtSurname.Name = "txtSurname";
            this.txtSurname.Size = new System.Drawing.Size(100, 23);
            this.txtSurname.TabIndex = 3;
            // 
            // lblSurname
            // 
            this.lblSurname.AutoSize = true;
            this.lblSurname.Location = new System.Drawing.Point(112, 19);
            this.lblSurname.Name = "lblSurname";
            this.lblSurname.Size = new System.Drawing.Size(57, 15);
            this.lblSurname.TabIndex = 2;
            this.lblSurname.Text = "Surname:";
            // 
            // txtFirstName
            // 
            this.txtFirstName.Enabled = false;
            this.txtFirstName.Location = new System.Drawing.Point(6, 37);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(100, 23);
            this.txtFirstName.TabIndex = 1;
            // 
            // lblFirstName
            // 
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Location = new System.Drawing.Point(6, 19);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(67, 15);
            this.lblFirstName.TabIndex = 0;
            this.lblFirstName.Text = "First Name:";
            // 
            // gbSummary
            // 
            this.gbSummary.Location = new System.Drawing.Point(294, 354);
            this.gbSummary.Name = "gbSummary";
            this.gbSummary.Size = new System.Drawing.Size(427, 120);
            this.gbSummary.TabIndex = 2;
            this.gbSummary.TabStop = false;
            this.gbSummary.Text = "Portfolio Summary";
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
            this.ResumeLayout(false);

        }
    }
}
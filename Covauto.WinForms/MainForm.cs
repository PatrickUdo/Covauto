using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Covauto.Shared.DTOs;

namespace Covauto.WinForms
{
    public class MainForm : Form
    {
        private readonly HttpClient _httpClient;

        private Panel _loginPanel;
        private TextBox _emailTextBox;
        private TextBox _passwordTextBox;
        private Label _statusLabel;

        private Panel _mainPanel;
        private TabControl _tabControl;
        private DataGridView _reserveringenGrid;
        private DataGridView _autosGrid;

        private TextBox _naamTextBox;
        private TextBox _kentekenTextBox;
        private NumericUpDown _kilometerstandNumeric;
        private Button _saveAutoButton;
        private int _editingAutoId = 0;

        public MainForm()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5095/api/");
            SetupForm();
        }

        private void SetupForm()
        {
            this.Text = "Backoffice";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 250, 252);

            CreateLoginPanel();
            CreateMainPanel();

            this.Controls.Add(_loginPanel);
            this.Controls.Add(_mainPanel);
        }

        private void CreateLoginPanel()
        {
            _loginPanel = new Panel
            {
                Size = new Size(450, 400),
                Location = new Point(375, 200),
                BackColor = Color.White,
                Visible = true
            };

            _loginPanel.Paint += (s, e) =>
            {
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(200, 200, 200)), 0, 0, _loginPanel.Width - 1, _loginPanel.Height - 1);
            };

            var titleLabel = new Label
            {
                Text = "Backoffice",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                Size = new Size(410, 50),
                Location = new Point(20, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(30, 41, 59)
            };

            var subtitleLabel = new Label
            {
                Text = "Inloggen om door te gaan",
                Font = new Font("Segoe UI", 12),
                Size = new Size(410, 30),
                Location = new Point(20, 80),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(100, 116, 139)
            };

            var emailLabel = new Label
            {
                Text = "Email:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(40, 140),
                Size = new Size(100, 25),
                ForeColor = Color.FromArgb(30, 41, 59)
            };

            _emailTextBox = new TextBox
            {
                Location = new Point(40, 165),
                Size = new Size(370, 30),
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle
            };

            var passwordLabel = new Label
            {
                Text = "Wachtwoord:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(40, 210),
                Size = new Size(100, 25),
                ForeColor = Color.FromArgb(30, 41, 59)
            };

            _passwordTextBox = new TextBox
            {
                Location = new Point(40, 235),
                Size = new Size(370, 30),
                Font = new Font("Segoe UI", 11),
                UseSystemPasswordChar = true,
                BorderStyle = BorderStyle.FixedSingle
            };

            var loginButton = new Button
            {
                Text = "Inloggen",
                Location = new Point(40, 285),
                Size = new Size(370, 40),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            loginButton.FlatAppearance.BorderSize = 0;
            loginButton.Click += async (s, e) => await LoginAsync();

            var registerLink = new LinkLabel
            {
                Text = "Geen account? Ga naar de website",
                Location = new Point(40, 340),
                Size = new Size(370, 25),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 10),
                LinkColor = Color.FromArgb(59, 130, 246)
            };
            registerLink.Click += (s, e) => System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("http://localhost:5260/register") { UseShellExecute = true });

            _statusLabel = new Label
            {
                Location = new Point(40, 365),
                Size = new Size(370, 25),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 10)
            };

            _loginPanel.Controls.AddRange(new Control[] { titleLabel, subtitleLabel, emailLabel, _emailTextBox, passwordLabel, _passwordTextBox, loginButton, registerLink, _statusLabel });
        }

        private void CreateMainPanel()
        {
            _mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Visible = false,
                BackColor = Color.FromArgb(248, 250, 252)
            };

            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.White
            };

            var titleLabel = new Label
            {
                Text = "Backoffice",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Location = new Point(30, 20),
                Size = new Size(300, 30),
                ForeColor = Color.FromArgb(30, 41, 59)
            };

            var logoutButton = new Button
            {
                Text = "Uitloggen",
                Location = new Point(1050, 20),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(239, 68, 68),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            logoutButton.FlatAppearance.BorderSize = 0;
            logoutButton.Click += (s, e) => Logout();

            headerPanel.Controls.AddRange(new Control[] { titleLabel, logoutButton });

            _tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11),
                Padding = new Point(20, 8)
            };

            CreateReserveringenTab();
            CreateAutosTab();
            CreateAutoFormTab();

            _mainPanel.Controls.Add(headerPanel);
            _mainPanel.Controls.Add(_tabControl);
        }

        private void CreateReserveringenTab()
        {
            var tabPage = new TabPage("Reserveringen")
            {
                BackColor = Color.FromArgb(248, 250, 252),
                Padding = new Padding(20)
            };

            var buttonPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.Transparent
            };

            var refreshButton = CreateButton("Vernieuwen", new Point(20, 15), Color.FromArgb(34, 197, 94));
            refreshButton.Click += async (s, e) => await LoadReserveringenAsync();

            var deleteButton = CreateButton("Verwijderen", new Point(150, 15), Color.FromArgb(239, 68, 68));
            deleteButton.Click += async (s, e) => await DeleteSelectedReservering();

            buttonPanel.Controls.AddRange(new Control[] { refreshButton, deleteButton });

            _reserveringenGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 10),
                RowHeadersVisible = false,
                AllowUserToResizeRows = false
            };

            _reserveringenGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(59, 130, 246);
            _reserveringenGrid.DefaultCellStyle.SelectionForeColor = Color.White;
            _reserveringenGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249);
            _reserveringenGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            tabPage.Controls.Add(_reserveringenGrid);
            tabPage.Controls.Add(buttonPanel);
            _tabControl.TabPages.Add(tabPage);
        }

        private void CreateAutosTab()
        {
            var tabPage = new TabPage("Auto's")
            {
                BackColor = Color.FromArgb(248, 250, 252),
                Padding = new Padding(20)
            };

            var buttonPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.Transparent
            };

            var refreshButton = CreateButton("Vernieuwen", new Point(20, 15), Color.FromArgb(34, 197, 94));
            refreshButton.Click += async (s, e) => await LoadAutosAsync();

            var editButton = CreateButton("Bewerken", new Point(150, 15), Color.FromArgb(245, 158, 11));
            editButton.Click += (s, e) => EditSelectedAuto();

            var deleteButton = CreateButton("Verwijderen", new Point(280, 15), Color.FromArgb(239, 68, 68));
            deleteButton.Click += async (s, e) => await DeleteSelectedAuto();

            buttonPanel.Controls.AddRange(new Control[] { refreshButton, editButton, deleteButton });

            _autosGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 10),
                RowHeadersVisible = false,
                AllowUserToResizeRows = false
            };

            _autosGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(59, 130, 246);
            _autosGrid.DefaultCellStyle.SelectionForeColor = Color.White;
            _autosGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249);
            _autosGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            tabPage.Controls.Add(_autosGrid);
            tabPage.Controls.Add(buttonPanel);
            _tabControl.TabPages.Add(tabPage);
        }

        private void CreateAutoFormTab()
        {
            var tabPage = new TabPage("Auto Toevoegen")
            {
                BackColor = Color.FromArgb(248, 250, 252),
                Padding = new Padding(40)
            };

            var formPanel = new Panel
            {
                Size = new Size(500, 400),
                Location = new Point(50, 50),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var formTitle = new Label
            {
                Text = "Auto Informatie",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(30, 30),
                Size = new Size(200, 30),
                ForeColor = Color.FromArgb(30, 41, 59)
            };

            var naamLabel = CreateLabel("Naam:", new Point(30, 80));
            _naamTextBox = CreateTextBox(new Point(30, 105));

            var kentekenLabel = CreateLabel("Kenteken:", new Point(30, 150));
            _kentekenTextBox = CreateTextBox(new Point(30, 175));

            var kilometerstandLabel = CreateLabel("Kilometerstand:", new Point(30, 220));
            _kilometerstandNumeric = new NumericUpDown
            {
                Location = new Point(30, 245),
                Size = new Size(440, 30),
                Font = new Font("Segoe UI", 11),
                Minimum = 0,
                Maximum = 999999,
                Value = 0
            };

            _saveAutoButton = CreateButton("Opslaan", new Point(30, 300), Color.FromArgb(34, 197, 94));
            _saveAutoButton.Click += async (s, e) => await SaveAutoAsync();

            var clearButton = CreateButton("Leegmaken", new Point(160, 300), Color.FromArgb(107, 114, 128));
            clearButton.Click += (s, e) => ClearAutoForm();

            formPanel.Controls.AddRange(new Control[] {
                formTitle, naamLabel, _naamTextBox, kentekenLabel, _kentekenTextBox,
                kilometerstandLabel, _kilometerstandNumeric, _saveAutoButton, clearButton
            });

            tabPage.Controls.Add(formPanel);
            _tabControl.TabPages.Add(tabPage);
        }

        private Button CreateButton(string text, Point location, Color backColor)
        {
            var button = new Button
            {
                Text = text,
                Location = location,
                Size = new Size(120, 30),
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private Label CreateLabel(string text, Point location)
        {
            return new Label
            {
                Text = text,
                Location = location,
                Size = new Size(440, 25),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59)
            };
        }

        private TextBox CreateTextBox(Point location)
        {
            return new TextBox
            {
                Location = location,
                Size = new Size(440, 30),
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        private async Task LoginAsync()
        {
            try
            {
                _statusLabel.Text = "Inloggen...";
                _statusLabel.ForeColor = Color.FromArgb(59, 130, 246);

                var loginData = new LoginDto { Email = _emailTextBox.Text, Password = _passwordTextBox.Text };
                var json = JsonSerializer.Serialize(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    _loginPanel.Visible = false;
                    _mainPanel.Visible = true;
                    await LoadReserveringenAsync();
                    await LoadAutosAsync();
                }
                else
                {
                    _statusLabel.Text = "Inloggen mislukt. Controleer je gegevens.";
                    _statusLabel.ForeColor = Color.FromArgb(239, 68, 68);
                }
            }
            catch (Exception ex)
            {
                _statusLabel.Text = $"Fout: {ex.Message}";
                _statusLabel.ForeColor = Color.FromArgb(239, 68, 68);
            }
        }

        private async Task LoadReserveringenAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("LeenAutoReservering");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var reserveringen = JsonSerializer.Deserialize<List<LeenAutoReserveringDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    _reserveringenGrid.DataSource = reserveringen.Select(r => new
                    {
                        ID = r.Id,
                        WerknemerID = r.WerknemerId ?? "-",
                        Werknemer = string.IsNullOrEmpty(r.WerknemerUserName) ? "-" : r.WerknemerUserName,
                        Email = string.IsNullOrEmpty(r.WerknemerEmail) ? "-" : r.WerknemerEmail,
                        AutoID = r.AutoId,
                        StartTijd = r.StartDatum.ToString("dd-MM HH:mm"),
                        EindTijd = r.EindDatum.ToString("dd-MM HH:mm"),
                        Voltooid = r.RitVoltooid ? "Ja" : "Nee",
                        Van = string.IsNullOrEmpty(r.BeginAdres) ? "-" : r.BeginAdres,
                        Naar = string.IsNullOrEmpty(r.EindAdres) ? "-" : r.EindAdres
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij laden reserveringen: {ex.Message}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadAutosAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("Auto");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var autos = JsonSerializer.Deserialize<List<AutoDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    _autosGrid.DataSource = autos.Select(a => new
                    {
                        ID = a.Id,
                        Naam = a.Naam,
                        Kenteken = a.Kenteken,
                        Kilometerstand = a.Kilometerstand.ToString("#,##0") + " km"
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij laden auto's: {ex.Message}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task DeleteSelectedReservering()
        {
            if (_reserveringenGrid.SelectedRows.Count > 0)
            {
                var selectedRow = _reserveringenGrid.SelectedRows[0];
                var id = (int)selectedRow.Cells["ID"].Value;

                var result = MessageBox.Show("Weet je zeker dat je deze reservering wilt verwijderen?", "Bevestiging", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        var response = await _httpClient.DeleteAsync($"LeenAutoReservering/{id}");
                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Reservering verwijderd!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            await LoadReserveringenAsync();
                        }
                        else
                        {
                            MessageBox.Show("Fout bij verwijderen.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Fout: {ex.Message}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecteer eerst een reservering.", "Geen selectie", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void EditSelectedAuto()
        {
            if (_autosGrid.SelectedRows.Count > 0)
            {
                var selectedRow = _autosGrid.SelectedRows[0];
                var id = (int)selectedRow.Cells["ID"].Value;
                var naam = selectedRow.Cells["Naam"].Value.ToString();
                var kenteken = selectedRow.Cells["Kenteken"].Value.ToString();
                var kilometerstandText = selectedRow.Cells["Kilometerstand"].Value.ToString().Replace(" km", "").Replace(",", "");

                _editingAutoId = id;
                _naamTextBox.Text = naam;
                _kentekenTextBox.Text = kenteken;
                _kilometerstandNumeric.Value = int.Parse(kilometerstandText);
                _saveAutoButton.Text = "Bijwerken";
                _tabControl.SelectedIndex = 2;
            }
            else
            {
                MessageBox.Show("Selecteer eerst een auto.", "Geen selectie", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async Task DeleteSelectedAuto()
        {
            if (_autosGrid.SelectedRows.Count > 0)
            {
                var selectedRow = _autosGrid.SelectedRows[0];
                var id = (int)selectedRow.Cells["ID"].Value;

                var result = MessageBox.Show("Weet je zeker dat je deze auto wilt verwijderen?", "Bevestiging", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        var response = await _httpClient.DeleteAsync($"Auto/{id}");
                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Auto verwijderd!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            await LoadAutosAsync();
                        }
                        else
                        {
                            MessageBox.Show("Fout bij verwijderen.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Fout: {ex.Message}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecteer eerst een auto.", "Geen selectie", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ClearAutoForm()
        {
            _editingAutoId = 0;
            _naamTextBox.Clear();
            _kentekenTextBox.Clear();
            _kilometerstandNumeric.Value = 0;
            _saveAutoButton.Text = "Opslaan";
        }

        private async Task SaveAutoAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_naamTextBox.Text) || string.IsNullOrWhiteSpace(_kentekenTextBox.Text))
                {
                    MessageBox.Show("Vul alle velden in.", "Validatie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var autoDto = new AutoDTO
                {
                    Id = _editingAutoId,
                    Naam = _naamTextBox.Text.Trim(),
                    Kenteken = _kentekenTextBox.Text.Trim(),
                    Kilometerstand = (int)_kilometerstandNumeric.Value
                };

                var json = JsonSerializer.Serialize(autoDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response;
                if (_editingAutoId > 0)
                {
                    response = await _httpClient.PutAsync($"Auto/{autoDto.Id}", content);
                }
                else
                {
                    response = await _httpClient.PostAsync("Auto", content);
                }

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Auto succesvol {(_editingAutoId > 0 ? "bijgewerkt" : "toegevoegd")}!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearAutoForm();
                    await LoadAutosAsync();
                    _tabControl.SelectedIndex = 1;
                }
                else
                {
                    MessageBox.Show("Fout bij opslaan.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout: {ex.Message}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Logout()
        {
            _loginPanel.Visible = true;
            _mainPanel.Visible = false;
            _emailTextBox.Clear();
            _passwordTextBox.Clear();
            _statusLabel.Text = "";
            ClearAutoForm();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _httpClient?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
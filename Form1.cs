using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Formats.Tar;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ModVersionConvertor
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient client = new();

        public Form1()
        {
            InitializeComponent();
            // Set a user agent for Modrinth API politeness
            client.DefaultRequestHeaders.Add("User-Agent", "WinForms Mod Downloader (histarry233@gmail.com)");
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                var res =
                    await client.GetStringAsync
                    ("https://launchermeta.mojang.com/mc/game/version_manifest.json");

                var versions = JsonConvert.DeserializeObject<dynamic>(res);
                versions = versions["versions"];

                foreach (var version in versions)
                {
                    if (version["type"].ToString() == "release")
                    {
                        versionCombo.Items.Add(version["id"].ToString());
                    }
                }
            }
        }

        private void versionCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            loaderCombo.Items.Clear();
            loaderCombo.Enabled = true;

            int major = int.Parse(versionCombo.Text.Split(".")[1]);
            int minor = 0;
            if (versionCombo.Text.Split(".").Length > 2)
                minor = int.Parse(versionCombo.Text.Split(".")[2]);

            if (major > 20 || (major == 20 && minor >= 2))
            {
                loaderCombo.Items.Add("NeoForge");
            }

            if (major >= 14)
            {
                loaderCombo.Items.Add("Fabric");
            }


            if (major >= 14 || (major == 14 && minor >= 4))
            {
                loaderCombo.Items.Add("Quilt");
            }

            loaderCombo.Items.Add("Forge");
        }


        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    txtModsPath.Text = fbd.SelectedPath;
                }
            }
        }

        public async Task<string> HashFileAsync(string filePath, string algorithmName)
        {
            using (var hasher = HashAlgorithm.Create(algorithmName.ToUpperInvariant()))
            {
                if (hasher == null)
                {
                    throw new ArgumentException("Invalid or unsupported algorithm.", nameof(algorithmName));
                }

                using (var stream = File.OpenRead(filePath))
                {
                    byte[] digest = await hasher.ComputeHashAsync(stream);

                    var sb = new StringBuilder(digest.Length * 2);
                    foreach (byte b in digest)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
        }

        int successCount = 0;
        int failCount = 0;
        private ConcurrentBag<string> failedMods = new ConcurrentBag<string>();
        private async void btnStart_Click(object sender, EventArgs e)
        {
            // --- Input Validation ---
            if (string.IsNullOrWhiteSpace(txtModsPath.Text) || !Directory.Exists(txtModsPath.Text))
            {
                MessageBox.Show("Please select a valid mods folder.", "Invalid Path", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string modsPath = txtModsPath.Text;
            string mcVersion = versionCombo.Text;
            string loader = loaderCombo.Text;


            // --- UI Preparation ---
            btnStart.Enabled = false;
            btnBrowse.Enabled = false;
            groupBox1.Enabled = false;
            rtbLog.Clear();
            progressBar.Value = 0;
            progressBar.Step = 1;

            string outputDirectory = 
                txtOutput.Text == "" ?
                Path.Combine(Directory.GetParent(modsPath)?.FullName ?? "", $"{loader}_mods")
                : txtOutput.Text;
            Directory.CreateDirectory(outputDirectory);
            UpdateLog($"Output directory: {outputDirectory}\n");

            // --- Main Logic ---
            var jarFiles = Directory.GetFiles(modsPath, "*.jar");
            progressBar.Maximum = jarFiles.Length;

            var tasks = new List<Task>();
            foreach (string jarFile in jarFiles)
            {
                tasks.Add(FindAndDownload(mcVersion, loader, jarFile, outputDirectory));
            }

            await Task.WhenAll(tasks);

            if (checkBox1.Checked)
            {
                await AutoSinytra(mcVersion, loader, outputDirectory, failedMods);
            }

            // --- Finalization ---
            UpdateLog("\n--- Process Complete ---", Color.Black, true);
            UpdateLog($"Successfully downloaded: {successCount}", Color.Green, true);
            UpdateLog($"Failed to find/download: {failCount}", Color.Red, true);

            if (!failedMods.IsEmpty)
            {
                string failedModsPath = Path.Combine(outputDirectory, "failedConversions.txt");
                await File.WriteAllTextAsync(failedModsPath, string.Join(Environment.NewLine, failedMods));
                UpdateLog($"A list of mods that could not be converted has been saved to: {failedModsPath}", Color.OrangeRed, true);
            }

            btnStart.Enabled = true;
            btnBrowse.Enabled = true;
            groupBox1.Enabled = true;
            progressBar.Value = progressBar.Maximum;
        }

        public async Task<ModrinthFile> FindModFileByModId
            (string mcVersion, string loader, string modId, string versionNum = "")
        {
            string apiUrl = $"https://api.modrinth.com/v2/project/{modId}/version";
            var response = await client.GetStringAsync(apiUrl);


            var versions =
                JsonConvert.DeserializeObject<dynamic>(response.ToString());

            List<ModrinthFile> files = new();

            foreach (var v in versions)
            {
                string[] gameVer = JsonConvert.DeserializeObject<string[]>
                    (v.game_versions.ToString());


                string[] loaders = JsonConvert.DeserializeObject<string[]>
                    (v.loaders.ToString());


                dynamic[] availableFiles = JsonConvert.DeserializeObject<dynamic[]>
                    (v.files.ToString());

                if (!gameVer.Contains(mcVersion) || !loaders.Contains(loader.ToLower()))
                {
                    continue;
                }

                files.Add(new()
                {
                    Filename = availableFiles[0].filename.ToString(),
                    Version = v.version_number.ToString(),
                    Url = availableFiles[0].url.ToString(),
                    Date = DateTime.Parse(v.date_published.ToString())
                });
            }

            if (files.Count == 0)
            {
                UpdateLog($"   - No compatible {loader} version found for {modId} on {mcVersion}.", Color.OrangeRed);

                return null;
            }

            List<ModrinthFile> modFiles =
                files.OrderByDescending(v => v.Date).ToList();
            var latestVersion = modFiles[0];

            if (!string.IsNullOrEmpty(versionNum))
            {
                foreach (var file in modFiles)
                {
                    if (file.Version == versionNum)
                        return file;
                }
            }

            return latestVersion;
        }

        public async Task AutoSinytra(string mcVersion, string loader,
            string outputDirectory, ConcurrentBag<string> failedMods)
        {

            var sinytra = await FindModFileByModId(mcVersion, loader, "u58R1TMW");
            var forgifiedFabricApi = await FindModFileByModId(mcVersion, loader, "Aqlf1Shp");
            var connectorExtra = await FindModFileByModId(mcVersion, loader, "FYpiwiBR");


            UpdateLog($"   + Found version: {sinytra.Version}. Downloading...", Color.Blue);

            var fileBytes = await client.GetByteArrayAsync(sinytra.Url);
            string outputPath = Path.Combine(outputDirectory, "[SINYTRA] Sinytra Connector.jar");
            await File.WriteAllBytesAsync(outputPath, fileBytes);

            UpdateLog($"     Downloaded Sinytra Connector", Color.Green);

            UpdateLog($"   + Found version: {forgifiedFabricApi.Version}. Downloading...", Color.Blue);

            fileBytes = await client.GetByteArrayAsync(forgifiedFabricApi.Url);
            outputPath = Path.Combine(outputDirectory, "[SINYTRA] Forgified Fabric API.jar");
            await File.WriteAllBytesAsync(outputPath, fileBytes);

            UpdateLog($"     Downloaded Forgified Fabric API", Color.Green);

            UpdateLog($"   + Found version: {connectorExtra.Version}. Downloading...", Color.Blue);

            fileBytes = await client.GetByteArrayAsync(connectorExtra.Url);
            outputPath = Path.Combine(outputDirectory, "[SINYTRA] Connector Extras.jar");
            await File.WriteAllBytesAsync(outputPath, fileBytes);

            UpdateLog($"     Downloaded Connector Extras", Color.Green);

            foreach (var mod in failedMods)
            {
                File.Copy(mod, Path.Combine(outputDirectory,
                    "[FABRIC] " + Path.GetFileName(mod)));
            }

        }

        public async Task<bool> FindAndDownload(string mcVersion, string loader, string jarFile,
            string outputDirectory)
        {
            string modId = "";
            try
            {
                string sha1 = await HashFileAsync(jarFile, "sha1");


                var res = await client.GetAsync
                    ($"https://api.modrinth.com/v2/version_file/{sha1}?algorithm=sha1");

                if (res == null || !res.IsSuccessStatusCode)
                {
                    UpdateLog($"-> Could not find Mod ID in {Path.GetFileName(jarFile)}.", Color.OrangeRed);

                    Interlocked.Increment(ref failCount);
                    failedMods.Add(jarFile);
                    return false;
                }

                var queryRes = JsonConvert.DeserializeObject<dynamic>(await res.Content.ReadAsStringAsync());
                modId = queryRes.project_id.ToString();
                var verNum = queryRes.version_number.ToString();

                if (string.IsNullOrWhiteSpace(modId))
                {
                    UpdateLog($"-> Could not find Mod ID in {Path.GetFileName(jarFile)}.", Color.OrangeRed);

                    Interlocked.Increment(ref failCount);
                    failedMods.Add(jarFile);
                    return false;
                }

                UpdateLog($"Processing {modId}...");

                var modFile = await FindModFileByModId(mcVersion, loader, modId,
                    checkBox2.Checked ? "" : verNum);
                if (modFile == null)
                {
                    Interlocked.Increment(ref failCount);
                    failedMods.Add(jarFile);
                    return false;
                }


                UpdateLog($"   + Found version: {modFile.Version}. Downloading...", Color.Blue);

                var fileBytes = await client.GetByteArrayAsync(modFile.Url);
                string outputPath = Path.Combine(outputDirectory, modFile.Filename ?? $"{modId}.jar");
                await File.WriteAllBytesAsync(outputPath, fileBytes);

                UpdateLog($"     Downloaded and saved as {modFile.Filename}", Color.Green);

                Interlocked.Increment(ref successCount);
                BeginInvoke(() =>
                {
                    progressBar.PerformStep();
                });
                return true;
            }
            catch (Exception ex)
            {
                UpdateLog($"   - An error occurred processing {modId} ({Path.GetFileName(jarFile)}): {ex.Message}", Color.Red);
                Interlocked.Increment(ref failCount);
                failedMods.Add(jarFile);
                return false;
            }
        }

        // Helper method to safely update the RichTextBox from any thread
        private void UpdateLog(string message, Color? color = null, bool bold = false)
        {
            if (rtbLog.InvokeRequired)
            {
                rtbLog.Invoke(new Action<string, Color?, bool>(UpdateLog), message, color, bold);
                return;
            }

            rtbLog.SelectionStart = rtbLog.TextLength;
            rtbLog.SelectionLength = 0;

            rtbLog.SelectionColor = color ?? rtbLog.ForeColor;
            rtbLog.SelectionFont = new Font(rtbLog.Font, bold ? FontStyle.Bold : FontStyle.Regular);
            rtbLog.AppendText(message + Environment.NewLine);
            rtbLog.SelectionColor = rtbLog.ForeColor; // Reset color

            // Auto-scroll to the bottom
            rtbLog.ScrollToCaret();
        }

        private void loaderCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkBox1.Enabled = loaderCombo.Text.Contains("Forge");
        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    txtOutput.Text = fbd.SelectedPath;
                }
            }
        }
    }

    // Helper classes for JSON Deserialization
    public class FabricModJson
    {
        public string? Id { get; set; }
    }

    public class ModrinthVersion
    {
        public string? VersionNumber { get; set; }
        public DateTime DatePublished { get; set; }
        public List<ModrinthFile>? Files { get; set; }
    }

    public class ModrinthFile
    {
        public string? Filename { get; set; }
        public string? Version { get; set; }
        public string? Url { get; set; }
        public DateTime Date { get; set; }
    }
        
}

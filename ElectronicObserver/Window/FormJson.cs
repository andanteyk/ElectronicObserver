using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window
{
	public partial class FormJson : DockContent
	{


		// yyyyMMdd_hhmmssff[S|Q]@api_path.json
		private static readonly Regex FileNamePattern = new Regex(@"\d{8}_\d{8}([SQ])@(.*)\.json$", RegexOptions.Compiled);
		private const string AutoUpdateDisabledMessage = "<自動更新が無効になっています。Configから有効化してください。>";

		private Regex _apiPattern;

		private string _currentAPIPath;


		public FormJson(FormMain parent)
		{
			InitializeComponent();


			ConfigurationChanged();

			Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormJson]);
		}

		private void FormJson_Load(object sender, EventArgs e)
		{

			var o = APIObserver.Instance;

			o.RequestReceived += RequestReceived;
			o.ResponseReceived += ResponseReceived;

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		}


		void RequestReceived(string apiname, dynamic data)
		{

			if (!AutoUpdate.Checked)
				return;

			if (_apiPattern != null && !_apiPattern.Match(apiname).Success)
				return;


			LoadRequest(apiname, data);
		}

		void ResponseReceived(string apiname, dynamic data)
		{

			if (!AutoUpdate.Checked)
				return;

			if (_apiPattern != null && !_apiPattern.Match(apiname).Success)
				return;


			LoadResponse(apiname, data);
		}



		private void LoadRequest(string apiname, Dictionary<string, string> data)
		{

			JsonRawData.Text = apiname + " : Request\r\n" + string.Join("\r\n", data.Select(p => p.Key + "=" + p.Value));


			if (!UpdatesTree.Checked)
				return;


			JsonTreeView.BeginUpdate();


			JsonTreeView.Nodes.Clear();
			JsonTreeView.Nodes.Add(apiname);

			TreeNode root = new TreeNode("<Request> : {" + data.Count + "}")
			{
				Name = "<Request>"
			};
			root.Nodes.AddRange(data.Select(e => new TreeNode(e.Key + " : " + e.Value)).ToArray());

			JsonTreeView.Nodes.Add(root);


			JsonTreeView.EndUpdate();
			_currentAPIPath = apiname;
		}

		private void LoadResponse(string apiname, dynamic data)
		{


			JsonRawData.Text = (_currentAPIPath == apiname ? JsonRawData.Text + "\r\n\r\n" : "") + apiname + " : Response\r\n" + (data == null ? "" : data.ToString());

			if (!UpdatesTree.Checked)
				return;


			JsonTreeView.BeginUpdate();


			if (JsonTreeView.Nodes.Count == 0 || JsonTreeView.Nodes[0].Text != apiname)
			{
				JsonTreeView.Nodes.Clear();
				JsonTreeView.Nodes.Add(apiname);
			}

			var node = CreateNode("<Response>", data);
			CreateChildNode(node);
			JsonTreeView.Nodes.Add(node);


			JsonTreeView.EndUpdate();
			_currentAPIPath = apiname;
		}





		private void LoadFromFile(string path)
		{


			var match = FileNamePattern.Match(path);

			if (match.Success)
			{

				try
				{

					using (var sr = new StreamReader(path))
					{

						string data = sr.ReadToEnd();


						if (match.Groups[1].Value == "Q")
						{
							// request

							var parsedData = new Dictionary<string, string>();
							data = System.Web.HttpUtility.UrlDecode(data);

							foreach (string unit in data.Split("&".ToCharArray()))
							{
								string[] pair = unit.Split("=".ToCharArray());
								parsedData.Add(pair[0], pair[1]);
							}

							LoadRequest(match.Groups[2].Value.Replace('@', '/'), parsedData);


						}
						else if (match.Groups[1].Value == "S")
						{
							//response

							int head = data.IndexOfAny("[{".ToCharArray());
							if (head == -1)
								throw new ArgumentException("JSON の開始文字を検出できませんでした。");
							data = data.Substring(head);

							LoadResponse(match.Groups[2].Value.Replace('@', '/'), Codeplex.Data.DynamicJson.Parse(data));

						}

					}

				}
				catch (Exception ex)
				{

					Utility.ErrorReporter.SendErrorReport(ex, "JSON データの読み込みに失敗しました。");
				}


			}
		}


		private void CreateChildNode(TreeNode root)
		{
			dynamic json = root.Tag;

			if (json == null || !(json is Codeplex.Data.DynamicJson))
			{
				return;

			}
			else if (json.IsArray)
			{
				foreach (var elem in json)
				{
					root.Nodes.Add(CreateNode("", elem));
				}

			}
			else if (json.IsObject)
			{
				foreach (KeyValuePair<string, dynamic> elem in json)
				{
					root.Nodes.Add(CreateNode(elem.Key, elem.Value));
				}
			}
		}

		private TreeNode CreateNode(string name, dynamic data)
		{
			TreeNode node = new TreeNode
			{
				Tag = data,
				Name = name,
				Text = string.IsNullOrEmpty(name) ? "" : (name + " : ")
			};

			if (data == null)
			{
				node.Text += "null";

			}
			else if (data is string)
			{
				node.Text += "\"" + data + "\"";

			}
			else if (data is bool || data is double)
			{
				node.Text += data.ToString();

			}
			else if (data.IsArray)
			{
				int count = 0;
				foreach (var elem in data)
					count++;
				node.Text += "[" + count + "]";

			}
			else if (data.IsObject)
			{
				int count = 0;
				foreach (var elem in data)
					count++;
				node.Text += "{" + count + "}";

			}
			else
			{
				throw new NotImplementedException();

			}

			return node;
		}


		// ノードが展開されたときに、孫ノードを生成する(子ノードは生成済みという前提)
		// 子ノードまでだと [+] [-] が表示されないため孫まで生成しておく
		private void JsonTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{

			// Checked は展開済みフラグ :(
			if (e.Node.Checked)
				return;

			foreach (TreeNode child in e.Node.Nodes)
			{
				CreateChildNode(child);
			}

			e.Node.Checked = true;
		}



		void ConfigurationChanged()
		{

			var c = Utility.Configuration.Config;

			Font = tabControl1.Font = c.UI.MainFont;

			AutoUpdate.Checked = c.FormJson.AutoUpdate;
			UpdatesTree.Checked = c.FormJson.UpdatesTree;
			AutoUpdateFilter.Text = c.FormJson.AutoUpdateFilter;


			JsonTreeView.Nodes.Clear();

			if (!AutoUpdate.Checked || !UpdatesTree.Checked)
				JsonTreeView.Nodes.Add(AutoUpdateDisabledMessage);


			try
			{
				_apiPattern = new Regex(c.FormJson.AutoUpdateFilter);
				AutoUpdateFilter.BackColor = SystemColors.Window;

			}
			catch (Exception)
			{
				AutoUpdateFilter.BackColor = Color.MistyRose;
				_apiPattern = null;
			}
		}


		private void tabControl1_DragEnter(object sender, DragEventArgs e)
		{

			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Copy;
			else
				e.Effect = DragDropEffects.None;

		}

		private void tabControl1_DragDrop(object sender, DragEventArgs e)
		{

			foreach (string path in ((string[])e.Data.GetData(DataFormats.FileDrop)).OrderBy(s => s))
				LoadFromFile(path);

		}


		private void UpdatesTree_CheckedChanged(object sender, EventArgs e)
		{

			JsonTreeView.Nodes.Clear();

			if (!AutoUpdate.Checked || !UpdatesTree.Checked)
				JsonTreeView.Nodes.Add(AutoUpdateDisabledMessage);


			Utility.Configuration.Config.FormJson.UpdatesTree = UpdatesTree.Checked;
		}

		private void AutoUpdate_CheckedChanged(object sender, EventArgs e)
		{

			JsonTreeView.Nodes.Clear();

			if (!AutoUpdate.Checked || !UpdatesTree.Checked)
				JsonTreeView.Nodes.Add(AutoUpdateDisabledMessage);


			Utility.Configuration.Config.FormJson.AutoUpdate = AutoUpdate.Checked;
		}


		private void AutoUpdateFilter_Validated(object sender, EventArgs e)
		{
			var c = Utility.Configuration.Config.FormJson;
			c.AutoUpdateFilter = AutoUpdateFilter.Text;

			try
			{
				_apiPattern = new Regex(c.AutoUpdateFilter);
				AutoUpdateFilter.BackColor = SystemColors.Window;

			}
			catch (Exception)
			{
				AutoUpdateFilter.BackColor = Color.MistyRose;
				_apiPattern = null;
			}
		}


		private void TreeContextMenu_Expand_Click(object sender, EventArgs e)
		{
			JsonTreeView.SelectedNode.ExpandAll();
		}
		private void TreeContextMenu_Shrink_Click(object sender, EventArgs e)
		{
			JsonTreeView.SelectedNode.Collapse();
		}
		private void TreeContextMenu_ShrinkParent_Click(object sender, EventArgs e)
		{
			var node = JsonTreeView.SelectedNode.Parent;
			if (node != null)
				node.Collapse();
		}


		private void TreeContextMenu_Opening(object sender, CancelEventArgs e)
		{

			var root = JsonTreeView.SelectedNode;
			dynamic json = root.Tag;

			// root is array, children > 0, root[0](=child) is object or array
			if (
				root.GetNodeCount(false) > 0 &&
				json != null && json is Codeplex.Data.DynamicJson && json.IsArray &&
				root.FirstNode.Tag != null && root.FirstNode.Tag is Codeplex.Data.DynamicJson && (((dynamic)root.FirstNode.Tag).IsArray || ((dynamic)root.FirstNode.Tag).IsObject))
			{
				TreeContextMenu_OutputCSV.Enabled = true;

			}
			else
			{
				TreeContextMenu_OutputCSV.Enabled = false;
			}

		}

		private void TreeContextMenu_OutputCSV_Click(object sender, EventArgs e)
		{

			if (CSVSaver.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{

				try
				{

					using (var sw = new StreamWriter(CSVSaver.FileName, false, Utility.Configuration.Config.Log.FileEncoding))
					{

						var root = JsonTreeView.SelectedNode;

						sw.WriteLine(BuildCSVHeader(new StringBuilder(), "", ((dynamic)root.Tag)[0]).ToString());

						foreach (dynamic elem in (dynamic)root.Tag)
						{
							sw.WriteLine(BuildCSVContent(new StringBuilder(), elem).ToString());
						}
					}


				}
				catch (Exception ex)
				{
					Utility.ErrorReporter.SendErrorReport(ex, "JSON: CSV 出力に失敗しました。");
					MessageBox.Show("CSV 出力に失敗しました。\r\n" + ex.Message, "出力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}


			}

		}


		private StringBuilder BuildCSVHeader(StringBuilder sb, string currentPath, dynamic data)
		{

			if (data is Codeplex.Data.DynamicJson)
			{

				if (data.IsObject)
				{
					foreach (string p in data.GetDynamicMemberNames())
					{
						BuildCSVHeader(sb, currentPath + "." + p, data[p]);
					}
					return sb;

				}
				else if (data.IsArray)
				{
					int index = 0;
					foreach (dynamic elem in data)
					{
						BuildCSVHeader(sb, currentPath + "[" + index + "]", elem);
						index++;
					}
					return sb;

				}
			}

			sb.Append(currentPath).Append(",");
			return sb;
		}

		private StringBuilder BuildCSVContent(StringBuilder sb, dynamic data)
		{

			if (data is Codeplex.Data.DynamicJson)
			{

				if (data.IsObject)
				{
					foreach (string p in data.GetDynamicMemberNames())
					{
						BuildCSVContent(sb, data[p]);
					}
					return sb;

				}
				else if (data.IsArray)
				{
					foreach (dynamic elem in data)
					{
						BuildCSVContent(sb, elem);
					}
					return sb;

				}
			}

			sb.Append(data).Append(",");
			return sb;
		}


		private void TreeContextMenu_CopyToClipboard_Click(object sender, EventArgs e)
		{
			if (JsonTreeView.SelectedNode != null && JsonTreeView.SelectedNode.Tag != null)
			{
				Clipboard.SetData(DataFormats.StringFormat, JsonTreeView.SelectedNode.Tag.ToString());
			}
			else
			{
				System.Media.SystemSounds.Exclamation.Play();
			}
		}


		private StringBuilder BuildDocument(dynamic data)
		{
			return BuildDocumentContent(new StringBuilder(), data, 0);
		}

		private StringBuilder BuildDocumentContent(StringBuilder sb, dynamic data, int indentLevel)
		{
			if (data is Codeplex.Data.DynamicJson)
			{
				if (data.IsObject)
				{
					foreach (string p in data.GetDynamicMemberNames())
					{
						sb.AppendLine();
						for (int i = 0; i < indentLevel; i++)
							sb.Append("\t");
						sb.Append(p);

						int tab = (int)Math.Ceiling( (24 - (p.Length /*+ indentLevel * 4*/)) / 4.0);
						for (int i = 0; i < tab; i++)
							sb.Append("\t");
						sb.Append("：");

						BuildDocumentContent(sb, data[p], indentLevel + 1);
					}
				}
				else if (data.IsArray)
				{
					sb.Append($"[{((dynamic[])data).Length}]");

					foreach (dynamic elem in data)
					{
						if (elem is Codeplex.Data.DynamicJson && (elem.IsObject || elem.IsArray))
						{
							BuildDocumentContent(sb, elem, indentLevel);

							break;
						}
					}
				}
			}

			return sb;
		}

		private void TreeContextMenu_CopyAsDocument_Click(object sender, EventArgs e)
		{
			if (JsonTreeView.SelectedNode != null && JsonTreeView.SelectedNode.Tag != null)
			{
				Clipboard.SetData(DataFormats.StringFormat, BuildDocument(JsonTreeView.SelectedNode.Tag));
			}
			else
			{
				System.Media.SystemSounds.Exclamation.Play();
			}
		}



		// 右クリックでも選択するように
		private void JsonTreeView_MouseClick(object sender, MouseEventArgs e)
		{
			var node = JsonTreeView.GetNodeAt(e.Location);
			if (node != null)
			{
				JsonTreeView.SelectedNode = node;
			}
		}



		protected override string GetPersistString()
		{
			return "Json";
		}


	}
}

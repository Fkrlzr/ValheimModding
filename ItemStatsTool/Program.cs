using System.Globalization;
using ClosedXML.Excel;

// Builds ValheimItemStats.xlsx from the CSV written by the ItemStatsDump plugin.
// Usage: ItemStatsTool [items.csv] [output.xlsx] [--verify]
const int MaxLevel = 65535;
const string Calc = "Calculator";
const string LevelCell = "Calculator!$C$3";

string baseDir = @"E:\Documents\Sekolah\ValheimModding\ItemStats";
string csvPath = args.Length > 0 && !args[0].StartsWith("--") ? args[0] : Path.Combine(baseDir, "items.csv");
string outPath = args.Length > 1 && !args[1].StartsWith("--") ? args[1] : Path.Combine(baseDir, "ValheimItemStats.xlsx");
bool verify = args.Contains("--verify");

string[] textColumns = { "Prefab", "Name", "Type", "Skill", "Resistances" };
string[] weaponTypes = { "OneHandedWeapon", "TwoHandedWeapon", "TwoHandedWeaponLeft", "Bow", "Tool", "Torch" };
string[] armorTypes = { "Helmet", "Chest", "Legs", "Shoulder", "Shield" };
string[] damageTypes = { "Blunt", "Slash", "Pierce", "Chop", "Pickaxe", "Fire", "Frost", "Lightning", "Poison", "Spirit", "Damage" };

// ---- read CSV ----
List<List<string>> table = ParseCsv(File.ReadAllText(csvPath));
List<string> header = table[0];
int Idx(string name) => header.IndexOf(name) is var i and >= 0 ? i : throw new InvalidOperationException($"Missing column {name}");

var rows = new List<List<string>>();
foreach (List<string> r in table.Skip(1))
{
	if (r.Count < header.Count) continue;
	if (int.Parse(r[Idx("MaxQuality")], CultureInfo.InvariantCulture) <= 1) continue; // creature attacks etc, cannot be upgraded
	string type = r[Idx("Type")];
	if (!weaponTypes.Contains(type) && !armorTypes.Contains(type)) continue;
	rows.Add(r);
}

// Unique display key: the name, plus the prefab when two items share a name.
var nameCount = rows.GroupBy(r => r[Idx("Name")]).ToDictionary(g => g.Key, g => g.Count());
string Key(List<string> r) => nameCount[r[Idx("Name")]] > 1 ? $"{r[Idx("Name")]} ({r[Idx("Prefab")]})" : r[Idx("Name")];

// ---- Data sheet (column A = unique item key, then every CSV column) ----
using var wb = new XLWorkbook();
IXLWorksheet calc = wb.AddWorksheet(Calc);
IXLWorksheet weapons = wb.AddWorksheet("Weapons");
IXLWorksheet armor = wb.AddWorksheet("Armor");
IXLWorksheet data = wb.AddWorksheet("Data");

data.Cell(1, 1).Value = "Item";
for (int c = 0; c < header.Count; c++) data.Cell(1, c + 2).Value = header[c];
for (int i = 0; i < rows.Count; i++)
{
	data.Cell(i + 2, 1).Value = Key(rows[i]);
	for (int c = 0; c < header.Count; c++)
	{
		string v = rows[i][c];
		if (textColumns.Contains(header[c])) data.Cell(i + 2, c + 2).Value = v;
		else data.Cell(i + 2, c + 2).Value = double.Parse(v, CultureInfo.InvariantCulture);
	}
}
int lastData = rows.Count + 1;
data.Row(1).Style.Font.Bold = true;
data.SheetView.FreezeRows(1);
data.Range(1, 1, lastData, header.Count + 1).SetAutoFilter();
data.Columns().AdjustToContents();

string ColL(string name) => XLHelper.GetColumnLetterFromNumber(Idx(name) + 2);
string D(string name, int row) => $"Data!${ColL(name)}${row}";
string Range(string name) => $"Data!${ColL(name)}$2:${ColL(name)}${lastData}";
string KeyRange = $"Data!$A$2:$A${lastData}";

// ---- Calculator ----
calc.Cell("A1").Value = "Valheim item stats calculator";
calc.Cell("A1").Style.Font.Bold = true;
calc.Cell("A1").Style.Font.FontSize = 16;
calc.Cell("B3").Value = $"Level (1 - {MaxLevel})";
calc.Cell("C3").Value = 4;
calc.Cell("B4").Value = "Item";
string defaultItem = rows.Select(Key).FirstOrDefault(k => k == "Iron Sword") ?? Key(rows[0]);
calc.Cell("C4").Value = defaultItem;
calc.Cell("D3").Value = "Change the level here; the Weapons and Armor sheets update too.";
calc.Cell("D4").Value = "Pick an item from the dropdown.";
calc.Range("D3:D4").Style.Font.Italic = true;
calc.Range("C3:C4").Style.Fill.BackgroundColor = XLColor.LightYellow;
calc.Range("C3:C4").Style.Font.Bold = true;
calc.Cell("C3").CreateDataValidation().WholeNumber.Between(1, MaxLevel);
calc.Cell("C3").GetDataValidation().ErrorTitle = "Invalid level";
calc.Cell("C3").GetDataValidation().ErrorMessage = $"Enter a whole number from 1 to {MaxLevel}.";
calc.Cell("C4").CreateDataValidation().List(data.Range(2, 1, lastData, 1));

string[] statHeader = { "Stat", "Base (level 1)", "Per level", "At level" };
for (int i = 0; i < statHeader.Length; i++) calc.Cell(7, i + 2).Value = statHeader[i];

string Lookup(string col) => $"INDEX({Range(col)},MATCH($C$4,{KeyRange},0))";
int r0 = 8;
int row = r0;
foreach (string dt in damageTypes)
{
	calc.Cell(row, 2).Value = dt == "Damage" ? "Generic damage" : $"{dt} damage";
	calc.Cell(row, 3).FormulaA1 = "=" + Lookup(dt);
	calc.Cell(row, 4).FormulaA1 = "=" + Lookup(dt + "PL");
	row++;
}
int dmgFirst = r0, dmgLast = row - 1;
int totalDmgRow = row++;
calc.Cell(totalDmgRow, 2).Value = "Total damage";
calc.Cell(totalDmgRow, 3).FormulaA1 = $"=SUM(C{dmgFirst}:C{dmgLast})";
calc.Cell(totalDmgRow, 4).FormulaA1 = $"=SUM(D{dmgFirst}:D{dmgLast})";
calc.Row(totalDmgRow).Style.Font.Bold = true;
var singles = new (string Label, string Base, string Per)[]
{
	("Armor", "Armor", "ArmorPL"),
	("Block power", "BlockPower", "BlockPowerPL"),
	("Deflection force", "Deflection", "DeflectionPL"),
	("Durability", "Durability", "DurabilityPL"),
};
var statRows = new Dictionary<string, int> { ["Total damage"] = totalDmgRow };
foreach ((string label, string b, string p) in singles)
{
	calc.Cell(row, 2).Value = label;
	calc.Cell(row, 3).FormulaA1 = "=" + Lookup(b);
	calc.Cell(row, 4).FormulaA1 = "=" + Lookup(p);
	statRows[label] = row++;
}
calc.Cell(row, 2).Value = "Weight";
calc.Cell(row, 3).FormulaA1 = "=" + Lookup("Weight");
calc.Cell(row, 4).FormulaA1 = $"={Lookup("Weight")}*{Lookup("WeightScaleByQuality")}";
statRows["Weight"] = row++;
int lastStat = row - 1;
for (int r = r0; r <= lastStat; r++)
{
	// stat = base + max(0, level - 1) * perLevel (same formula the game uses for every stat above, weight included)
	calc.Cell(r, 5).FormulaA1 = $"=C{r}+MAX(0,$C$3-1)*D{r}";
}
calc.Range(7, 2, 7, 5).Style.Font.Bold = true;
calc.Range(7, 2, 7, 5).Style.Fill.BackgroundColor = XLColor.LightGray;
calc.Range(r0, 3, lastStat, 5).Style.NumberFormat.Format = "#,##0.##";
calc.Range(r0, 5, lastStat, 5).Style.Font.Bold = true;

// progression table
calc.Cell("G3").Value = "Progression start level";
calc.Cell("H3").Value = 4;
calc.Cell("G4").Value = "Step";
calc.Cell("H4").Value = 1;
calc.Range("H3:H4").Style.Fill.BackgroundColor = XLColor.LightYellow;
calc.Range("H3:H4").Style.Font.Bold = true;
calc.Cell("H3").CreateDataValidation().WholeNumber.Between(1, MaxLevel);
calc.Cell("H4").CreateDataValidation().WholeNumber.Between(1, MaxLevel);
string[] progHeader = { "Level", "Total damage", "Armor", "Block power", "Durability", "Weight" };
string[] progStat = { "", "Total damage", "Armor", "Block power", "Durability", "Weight" };
int progHeaderRow = 7, progRows = 50;
for (int i = 0; i < progHeader.Length; i++) calc.Cell(progHeaderRow, 7 + i).Value = progHeader[i];
for (int i = 0; i < progRows; i++)
{
	int r = progHeaderRow + 1 + i;
	calc.Cell(r, 7).FormulaA1 = $"=MIN({MaxLevel},$H$3+$H$4*{i})";
	for (int c = 1; c < progStat.Length; c++)
	{
		int sr = statRows[progStat[c]];
		calc.Cell(r, 7 + c).FormulaA1 = $"=$C${sr}+MAX(0,$G{r}-1)*$D${sr}";
	}
}
calc.Range(progHeaderRow, 7, progHeaderRow, 12).Style.Font.Bold = true;
calc.Range(progHeaderRow, 7, progHeaderRow, 12).Style.Fill.BackgroundColor = XLColor.LightGray;
calc.Range(progHeaderRow + 1, 8, progHeaderRow + progRows, 12).Style.NumberFormat.Format = "#,##0.##";
calc.Range(progHeaderRow + 1, 7, progHeaderRow + progRows, 7).Style.NumberFormat.Format = "#,##0";
calc.Column(1).Width = 3;
calc.Column(2).Width = 24;
calc.Column(3).Width = 28;
calc.Columns(4, 5).Width = 14;
calc.Column(6).Width = 3;
calc.Column(7).Width = 24;
calc.Columns(8, 12).Width = 14;

// ---- Weapons sheet ----
string lvl = $"MAX(0,{LevelCell}-1)";
string[] wHeader = { "Item", "Type", "Skill" };
var wExtra = new[] { "Total damage", "Block power", "Deflection force", "Durability", "Weight", "Knockback", "Backstab bonus" };
weapons.Cell(1, 1).Value = "Weapon and tool stats at the level set on the Calculator sheet:";
weapons.Cell(1, 5).FormulaA1 = $"={LevelCell}";
weapons.Cell(1, 1).Style.Font.Bold = true;
weapons.Cell(1, 5).Style.Font.Bold = true;
int wh = 3;
int wc = 1;
foreach (string h in wHeader) weapons.Cell(wh, wc++).Value = h;
foreach (string dt in damageTypes) weapons.Cell(wh, wc++).Value = dt == "Damage" ? "Generic" : dt;
foreach (string h in wExtra) weapons.Cell(wh, wc++).Value = h;
int wDmgFirst = wHeader.Length + 1, wDmgLast = wHeader.Length + damageTypes.Length;
int wr = wh + 1;
for (int i = 0; i < rows.Count; i++)
{
	if (!weaponTypes.Contains(rows[i][Idx("Type")])) continue;
	int dr = i + 2;
	int c = 1;
	weapons.Cell(wr, c++).FormulaA1 = $"=Data!$A${dr}";
	weapons.Cell(wr, c++).FormulaA1 = "=" + D("Type", dr);
	weapons.Cell(wr, c++).FormulaA1 = "=" + D("Skill", dr);
	foreach (string dt in damageTypes) weapons.Cell(wr, c++).FormulaA1 = $"={D(dt, dr)}+{lvl}*{D(dt + "PL", dr)}";
	weapons.Cell(wr, c++).FormulaA1 = $"=SUM({weapons.Cell(wr, wDmgFirst).Address}:{weapons.Cell(wr, wDmgLast).Address})";
	weapons.Cell(wr, c++).FormulaA1 = $"={D("BlockPower", dr)}+{lvl}*{D("BlockPowerPL", dr)}";
	weapons.Cell(wr, c++).FormulaA1 = $"={D("Deflection", dr)}+{lvl}*{D("DeflectionPL", dr)}";
	weapons.Cell(wr, c++).FormulaA1 = $"={D("Durability", dr)}+{lvl}*{D("DurabilityPL", dr)}";
	weapons.Cell(wr, c++).FormulaA1 = $"={D("Weight", dr)}*(1+{lvl}*{D("WeightScaleByQuality", dr)})";
	weapons.Cell(wr, c++).FormulaA1 = "=" + D("Knockback", dr);
	weapons.Cell(wr, c++).FormulaA1 = "=" + D("Backstab", dr);
	wr++;
}
int wLast = wr - 1;
FinishSheet(weapons, wh, wLast, wHeader.Length + damageTypes.Length + wExtra.Length, wHeader.Length + 1);

// ---- Armor sheet ----
string[] aHeader = { "Item", "Type", "Armor", "Block power", "Deflection force", "Durability", "Weight", "Resistances" };
armor.Cell(1, 1).Value = "Armor and shield stats at the level set on the Calculator sheet:";
armor.Cell(1, 5).FormulaA1 = $"={LevelCell}";
armor.Cell(1, 1).Style.Font.Bold = true;
armor.Cell(1, 5).Style.Font.Bold = true;
int ah = 3;
for (int i = 0; i < aHeader.Length; i++) armor.Cell(ah, i + 1).Value = aHeader[i];
int ar = ah + 1;
for (int i = 0; i < rows.Count; i++)
{
	if (!armorTypes.Contains(rows[i][Idx("Type")])) continue;
	int dr = i + 2;
	int c = 1;
	armor.Cell(ar, c++).FormulaA1 = $"=Data!$A${dr}";
	armor.Cell(ar, c++).FormulaA1 = "=" + D("Type", dr);
	armor.Cell(ar, c++).FormulaA1 = $"={D("Armor", dr)}+{lvl}*{D("ArmorPL", dr)}";
	armor.Cell(ar, c++).FormulaA1 = $"={D("BlockPower", dr)}+{lvl}*{D("BlockPowerPL", dr)}";
	armor.Cell(ar, c++).FormulaA1 = $"={D("Deflection", dr)}+{lvl}*{D("DeflectionPL", dr)}";
	armor.Cell(ar, c++).FormulaA1 = $"={D("Durability", dr)}+{lvl}*{D("DurabilityPL", dr)}";
	armor.Cell(ar, c++).FormulaA1 = $"={D("Weight", dr)}*(1+{lvl}*{D("WeightScaleByQuality", dr)})";
	armor.Cell(ar, c++).FormulaA1 = "=" + D("Resistances", dr);
	ar++;
}
FinishSheet(armor, ah, ar - 1, aHeader.Length, 3);
armor.Column(8).Width = 40;

wb.SaveAs(outPath);
Console.WriteLine($"Wrote {outPath}: {rows.Count} items ({wLast - wh} weapons/tools, {ar - 1 - ah} armor/shields)");

if (verify) Verify();
return;

// ---------- helpers ----------
void FinishSheet(IXLWorksheet ws, int headerRow, int lastRow, int colCount, int numberFrom)
{
	IXLRange head = ws.Range(headerRow, 1, headerRow, colCount);
	head.Style.Font.Bold = true;
	head.Style.Fill.BackgroundColor = XLColor.LightGray;
	ws.Range(headerRow, 1, Math.Max(lastRow, headerRow), colCount).SetAutoFilter();
	ws.SheetView.FreezeRows(headerRow);
	ws.SheetView.FreezeColumns(1);
	ws.Range(headerRow + 1, numberFrom, Math.Max(lastRow, headerRow + 1), colCount).Style.NumberFormat.Format = "#,##0.##";
	ws.Column(1).Width = 34;
	for (int c = 2; c <= colCount; c++) ws.Column(c).Width = 13;
}

void Verify()
{
	// Re-open the saved file and compare spreadsheet results against the game's formulas computed in C#.
	using var w = new XLWorkbook(outPath);
	IXLWorksheet wsCalc = w.Worksheet(Calc);
	IXLWorksheet wsWeap = w.Worksheet("Weapons");
	IXLWorksheet wsArm = w.Worksheet("Armor");
	double Num(List<string> r, string col) => double.Parse(r[Idx(col)], CultureInfo.InvariantCulture);
	int fails = 0, checks = 0;
	void Check(string what, double expected, double actual)
	{
		checks++;
		if (Math.Abs(expected - actual) > 1e-6 * Math.Max(1, Math.Abs(expected)))
		{
			fails++;
			if (fails <= 20) Console.WriteLine($"MISMATCH {what}: expected {expected}, got {actual}");
		}
	}

	foreach (int level in new[] { 1, 4, 5, 10, 100, 65535 })
	{
		wsCalc.Cell("C3").Value = level;
		int lv = Math.Max(0, level - 1);
		int wrow = wh + 1, arow = ah + 1;
		for (int i = 0; i < rows.Count; i++)
		{
			List<string> r = rows[i];
			string type = r[Idx("Type")];
			string who = $"{Key(r)} @ {level}";
			if (weaponTypes.Contains(type))
			{
				double total = 0;
				for (int d = 0; d < damageTypes.Length; d++)
				{
					double exp = Num(r, damageTypes[d]) + lv * Num(r, damageTypes[d] + "PL");
					total += exp;
					Check($"{who} {damageTypes[d]}", exp, wsWeap.Cell(wrow, wHeader.Length + 1 + d).GetDouble());
				}
				int c = wHeader.Length + damageTypes.Length + 1;
				Check($"{who} total", total, wsWeap.Cell(wrow, c).GetDouble());
				Check($"{who} block", Num(r, "BlockPower") + lv * Num(r, "BlockPowerPL"), wsWeap.Cell(wrow, c + 1).GetDouble());
				Check($"{who} durability", Num(r, "Durability") + lv * Num(r, "DurabilityPL"), wsWeap.Cell(wrow, c + 3).GetDouble());
				Check($"{who} weight", Num(r, "Weight") * (1 + lv * Num(r, "WeightScaleByQuality")), wsWeap.Cell(wrow, c + 4).GetDouble());
				wrow++;
			}
			else
			{
				Check($"{who} armor", Num(r, "Armor") + lv * Num(r, "ArmorPL"), wsArm.Cell(arow, 3).GetDouble());
				Check($"{who} block", Num(r, "BlockPower") + lv * Num(r, "BlockPowerPL"), wsArm.Cell(arow, 4).GetDouble());
				Check($"{who} durability", Num(r, "Durability") + lv * Num(r, "DurabilityPL"), wsArm.Cell(arow, 6).GetDouble());
				Check($"{who} weight", Num(r, "Weight") * (1 + lv * Num(r, "WeightScaleByQuality")), wsArm.Cell(arow, 7).GetDouble());
				arow++;
			}
		}

		// Calculator single-item lookup (default item)
		List<string> item = rows.First(x => Key(x) == defaultItem);
		double tot = damageTypes.Sum(d => Num(item, d) + lv * Num(item, d + "PL"));
		Check($"Calculator total damage {defaultItem} @ {level}", tot, wsCalc.Cell(totalDmgRow, 5).GetDouble());
		Check($"Calculator durability @ {level}", Num(item, "Durability") + lv * Num(item, "DurabilityPL"), wsCalc.Cell(statRows["Durability"], 5).GetDouble());
	}
	Console.WriteLine($"Verify: {checks} checks, {fails} mismatches");
	if (fails > 0) Environment.ExitCode = 1;
}

static List<List<string>> ParseCsv(string text)
{
	var result = new List<List<string>>();
	var row = new List<string>();
	var sb = new System.Text.StringBuilder();
	bool quoted = false;
	for (int i = 0; i < text.Length; i++)
	{
		char ch = text[i];
		if (quoted)
		{
			if (ch == '"')
			{
				if (i + 1 < text.Length && text[i + 1] == '"') { sb.Append('"'); i++; }
				else quoted = false;
			}
			else sb.Append(ch);
		}
		else if (ch == '"') quoted = true;
		else if (ch == ',') { row.Add(sb.ToString()); sb.Clear(); }
		else if (ch == '\n' || ch == '\r')
		{
			if (ch == '\r' && i + 1 < text.Length && text[i + 1] == '\n') i++;
			row.Add(sb.ToString()); sb.Clear();
			if (row.Count > 1 || row[0].Length > 0) result.Add(row);
			row = new List<string>();
		}
		else sb.Append(ch);
	}
	if (sb.Length > 0 || row.Count > 0) { row.Add(sb.ToString()); result.Add(row); }
	return result;
}

namespace DW4Lib.Events;

/// <summary>
/// Chapter 1 (Ragnar) event scripts.
/// Contains all scripted events, dialogs, and triggers for Chapter 1.
/// </summary>
public static class Chapter1Events {
	// ============================================================
	// Script IDs
	// ============================================================

	/// <summary>Chapter 1 script ID base.</summary>
	public const ushort ScriptBase = 0x0100;

	/// <summary>Chapter intro script.</summary>
	public const ushort IntroScript = ScriptBase + 0x00;

	/// <summary>King speaks to Ragnar.</summary>
	public const ushort KingMission = ScriptBase + 0x01;

	/// <summary>Finding the children info.</summary>
	public const ushort ChildrenInfo = ScriptBase + 0x02;

	/// <summary>Meet Healie.</summary>
	public const ushort MeetHealie = ScriptBase + 0x03;

	/// <summary>Healie joins party.</summary>
	public const ushort HealieJoins = ScriptBase + 0x04;

	/// <summary>Find Flying Shoes.</summary>
	public const ushort FlyingShoes = ScriptBase + 0x05;

	/// <summary>Loch Tower entrance.</summary>
	public const ushort LochTowerEntry = ScriptBase + 0x06;

	/// <summary>Saro's Shadow battle.</summary>
	public const ushort SaroShadowBattle = ScriptBase + 0x07;

	/// <summary>Children rescued.</summary>
	public const ushort ChildrenRescued = ScriptBase + 0x08;

	/// <summary>Return to King.</summary>
	public const ushort ReturnToKing = ScriptBase + 0x09;

	/// <summary>Chapter complete.</summary>
	public const ushort ChapterComplete = ScriptBase + 0x0A;

	// ============================================================
	// Flag IDs
	// ============================================================

	/// <summary>King gave mission.</summary>
	public const ushort FlagKingMission = 0x0001;

	/// <summary>Talked to villagers about children.</summary>
	public const ushort FlagChildrenInfo = 0x0002;

	/// <summary>Met Healie in well.</summary>
	public const ushort FlagMetHealie = 0x0003;

	/// <summary>Healie joined party.</summary>
	public const ushort FlagHealieJoined = 0x0004;

	/// <summary>Found Flying Shoes.</summary>
	public const ushort FlagFlyingShoes = 0x0005;

	/// <summary>Entered Loch Tower.</summary>
	public const ushort FlagLochTower = 0x0006;

	/// <summary>Defeated Saro's Shadow.</summary>
	public const ushort FlagSaroDefeated = 0x0007;

	/// <summary>Rescued children.</summary>
	public const ushort FlagChildrenRescued = 0x0008;

	/// <summary>Reported to King.</summary>
	public const ushort FlagReportedKing = 0x0009;

	/// <summary>Chapter 1 complete.</summary>
	public const ushort FlagChapterComplete = 0x000A;

	// ============================================================
	// Battle IDs
	// ============================================================

	/// <summary>Saro's Shadow boss battle.</summary>
	public const ushort BattleSaroShadow = 0x0001;

	// ============================================================
	// Music IDs
	// ============================================================

	/// <summary>Chapter 1 overworld theme.</summary>
	public const byte MusicOverworld = 0x10;

	/// <summary>Burland Castle theme.</summary>
	public const byte MusicCastle = 0x11;

	/// <summary>Town theme.</summary>
	public const byte MusicTown = 0x12;

	/// <summary>Dungeon theme.</summary>
	public const byte MusicDungeon = 0x13;

	/// <summary>Boss battle theme.</summary>
	public const byte MusicBoss = 0x14;

	/// <summary>Victory fanfare.</summary>
	public const byte MusicVictory = 0x15;

	// ============================================================
	// Shop IDs
	// ============================================================

	/// <summary>Burland weapon shop.</summary>
	public const byte ShopBurlandWeapon = 0x01;

	/// <summary>Burland armor shop.</summary>
	public const byte ShopBurlandArmor = 0x02;

	/// <summary>Burland item shop.</summary>
	public const byte ShopBurlandItem = 0x03;

	// ============================================================
	// Pre-built Scripts
	// ============================================================

	/// <summary>
	/// Get all Chapter 1 event scripts.
	/// </summary>
	public static EventScript[] GetAllScripts() => [
		BuildIntroScript(),
		BuildKingMissionScript(),
		BuildMeetHealieScript(),
		BuildHealieJoinsScript(),
		BuildFlyingShoesScript(),
		BuildLochTowerScript(),
		BuildSaroShadowBattleScript(),
		BuildChildrenRescuedScript(),
		BuildReturnToKingScript(),
		BuildChapterCompleteScript(),
		// Service scripts
		BuildWeaponShopScript(),
		BuildArmorShopScript(),
		BuildItemShopScript(),
		BuildInnScript(),
		BuildChurchScript()
	];

	/// <summary>
	/// Chapter intro - Ragnar receives his mission.
	/// </summary>
	public static EventScript BuildIntroScript() {
		return new EventScriptBuilder(IntroScript)
			.WithName("Chapter 1 Intro")
			.WithCategory(ScriptCategory.Cutscene)
			.ForChapter(0)
			// Fade in to Burland Castle throne room
			.FadeOut(8)
			.Wait(60)
			.PlayMusic(MusicCastle)
			.FadeIn(8)
			.Wait(30)
			// King speaks
			.ShowDialog(0x0001) // "Ragnar! Come forth!"
			.Wait(30)
			.ShowDialog(0x0002) // "Children have gone missing..."
			.Wait(30)
			.ShowDialog(0x0003) // "Find them and bring them back!"
			.SetFlag(FlagKingMission)
			.End()
			.Build();
	}

	/// <summary>
	/// King gives Ragnar his mission.
	/// </summary>
	public static EventScript BuildKingMissionScript() {
		return new EventScriptBuilder(KingMission)
			.WithName("King Mission")
			.WithCategory(ScriptCategory.Dialog)
			.ForChapter(0)
			.CheckFlag(FlagKingMission, 0x0010) // If already got mission, jump to alternate dialog
			// First time
			.ShowDialog(0x0010) // "The missing children..."
			.ShowDialog(0x0011) // "They say monsters took them..."
			.ShowDialog(0x0012) // "You must save them, Ragnar!"
			.SetFlag(FlagKingMission)
			.GiveGold(100) // Starting gold
			.End()
			// Jump target - return visit
			.ShowDialog(0x0013) // "Have you found the children?"
			.End()
			.Build();
	}

	/// <summary>
	/// Meeting Healie in the well.
	/// </summary>
	public static EventScript BuildMeetHealieScript() {
		return new EventScriptBuilder(MeetHealie)
			.WithName("Meet Healie")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(0)
			.CheckFlag(FlagMetHealie, 0x0020) // Already met
			// First meeting
			.ShowDialog(0x0020) // "Hey! Down here!"
			.ShowNpc(0xC5) // Show Healie sprite
			.ShowDialog(0x0021) // "I'm Healie! I was trapped!"
			.ShowDialog(0x0022) // "You saved me! Let me help you!"
			.SetFlag(FlagMetHealie)
			.JumpSubroutine(HealieJoins) // Healie offers to join
			.End()
			// Already met
			.ShowDialog(0x0023) // "Thanks again for rescuing me!"
			.End()
			.Build();
	}

	/// <summary>
	/// Healie joins the party.
	/// </summary>
	public static EventScript BuildHealieJoinsScript() {
		return new EventScriptBuilder(HealieJoins)
			.WithName("Healie Joins")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(0)
			.ShowChoice(0x0024, 2) // "Will you let me help? Yes/No"
			.CheckFlag(0x0000, 0x0030) // Branch on choice (flag 0 = choice result)
			// Yes
			.ShowDialog(0x0025) // "Yay! I'll do my best!"
			.AddPartyMember(0xC5) // Add Healie as NPC companion
			.SetFlag(FlagHealieJoined)
			.PlaySound(0x10) // Join sound effect
			.Return()
			// No
			.ShowDialog(0x0026) // "Oh... I understand..."
			.Return()
			.Build();
	}

	/// <summary>
	/// Finding the Flying Shoes.
	/// </summary>
	public static EventScript BuildFlyingShoesScript() {
		return new EventScriptBuilder(FlyingShoes)
			.WithName("Flying Shoes")
			.WithCategory(ScriptCategory.Item)
			.ForChapter(0)
			.CheckFlag(FlagFlyingShoes, 0x0040) // Already found
			// Find shoes
			.ShowDialog(0x0030) // "You found the Flying Shoes!"
			.GiveItem(0x2A) // Flying Shoes item ID
			.SetFlag(FlagFlyingShoes)
			.PlaySound(0x11) // Item get sound
			.End()
			// Already obtained
			.ShowDialog(0x0031) // "The chest is empty."
			.End()
			.Build();
	}

	/// <summary>
	/// Entering Loch Tower.
	/// </summary>
	public static EventScript BuildLochTowerScript() {
		return new EventScriptBuilder(LochTowerEntry)
			.WithName("Loch Tower Entry")
			.WithCategory(ScriptCategory.Trigger)
			.ForChapter(0)
			.CheckFlag(FlagFlyingShoes, 0x0050) // Need shoes
			// Has shoes
			.ShowDialog(0x0040) // "The Flying Shoes lift you up!"
			.FadeOut(4)
			.Wait(30)
			.PlayMusic(MusicDungeon)
			.Warp(0x15, 5, 5, 0) // Loch Tower map
			.SetFlag(FlagLochTower)
			.FadeIn(4)
			.End()
			// No shoes
			.ShowDialog(0x0041) // "The tower floats high above..."
			.End()
			.Build();
	}

	/// <summary>
	/// Boss battle with Saro's Shadow.
	/// </summary>
	public static EventScript BuildSaroShadowBattleScript() {
		return new EventScriptBuilder(SaroShadowBattle)
			.WithName("Saro's Shadow Battle")
			.WithCategory(ScriptCategory.Battle)
			.ForChapter(0)
			.CheckFlag(FlagSaroDefeated, 0x0060) // Already won
			// Battle
			.ShowDialog(0x0050) // "A dark presence..."
			.FadeOut(4)
			.PlayMusic(MusicBoss)
			.StartBattle(BattleSaroShadow)
			.CheckFlag(0x0000, 0x0058) // Battle won flag
			// Won
			.FadeIn(4)
			.PlayMusic(MusicVictory)
			.ShowDialog(0x0051) // "The shadow fades!"
			.SetFlag(FlagSaroDefeated)
			.GiveExp(500) // Bonus EXP
			.JumpSubroutine(ChildrenRescued)
			.End()
			// Lost (shouldn't happen normally)
			.End()
			// Already defeated
			.End()
			.Build();
	}

	/// <summary>
	/// Children are rescued after defeating boss.
	/// </summary>
	public static EventScript BuildChildrenRescuedScript() {
		return new EventScriptBuilder(ChildrenRescued)
			.WithName("Children Rescued")
			.WithCategory(ScriptCategory.Cutscene)
			.ForChapter(0)
			.FadeOut(4)
			.Wait(30)
			.ShowDialog(0x0060) // "The children are safe!"
			.ShowDialog(0x0061) // "They were being held here."
			.ShowDialog(0x0062) // "Let's return to Burland!"
			.SetFlag(FlagChildrenRescued)
			.FadeIn(4)
			.Return()
			.Build();
	}

	/// <summary>
	/// Returning to the King after rescue.
	/// </summary>
	public static EventScript BuildReturnToKingScript() {
		return new EventScriptBuilder(ReturnToKing)
			.WithName("Return to King")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(0)
			.CheckFlag(FlagChildrenRescued, 0x0070) // Need to rescue first
			.CheckFlag(FlagReportedKing, 0x0078) // Already reported
			// Report success
			.ShowDialog(0x0070) // "Your Majesty! The children!"
			.ShowDialog(0x0071) // "You have done well, Ragnar!"
			.ShowDialog(0x0072) // "The kingdom owes you a debt."
			.GiveGold(500) // Reward
			.GiveItem(0x3F) // Sword of Miracles reward
			.SetFlag(FlagReportedKing)
			.JumpSubroutine(ChapterComplete)
			.End()
			// Not rescued yet
			.ShowDialog(0x0013) // "Have you found the children?"
			.End()
			// Already reported
			.ShowDialog(0x0073) // "You are a true hero!"
			.End()
			.Build();
	}

	/// <summary>
	/// Chapter 1 complete script.
	/// </summary>
	public static EventScript BuildChapterCompleteScript() {
		return new EventScriptBuilder(ChapterComplete)
			.WithName("Chapter 1 Complete")
			.WithCategory(ScriptCategory.Cutscene)
			.ForChapter(0)
			.ShowDialog(0x0080) // "And so Chapter 1 ends..."
			.ShowDialog(0x0081) // "Ragnar's tale continues..."
			.FadeOut(8)
			.Wait(120)
			.SetFlag(FlagChapterComplete)
			.SetChapter(1) // Advance to Chapter 2
			.End()
			.Build();
	}

	// ============================================================
	// Shop Scripts
	// ============================================================

	/// <summary>
	/// Build weapon shop script.
	/// </summary>
	public static EventScript BuildWeaponShopScript() {
		return new EventScriptBuilder((ushort)(ScriptBase + 0x20))
			.WithName("Burland Weapon Shop")
			.WithCategory(ScriptCategory.Shop)
			.ForChapter(0)
			.ShowDialog(0x0100) // "Welcome to my weapon shop!"
			.OpenShop(ShopBurlandWeapon)
			.ShowDialog(0x0101) // "Come again!"
			.End()
			.Build();
	}

	/// <summary>
	/// Build armor shop script.
	/// </summary>
	public static EventScript BuildArmorShopScript() {
		return new EventScriptBuilder((ushort)(ScriptBase + 0x21))
			.WithName("Burland Armor Shop")
			.WithCategory(ScriptCategory.Shop)
			.ForChapter(0)
			.ShowDialog(0x0102) // "Welcome! Need some armor?"
			.OpenShop(ShopBurlandArmor)
			.ShowDialog(0x0103) // "Safe travels!"
			.End()
			.Build();
	}

	/// <summary>
	/// Build item shop script.
	/// </summary>
	public static EventScript BuildItemShopScript() {
		return new EventScriptBuilder((ushort)(ScriptBase + 0x22))
			.WithName("Burland Item Shop")
			.WithCategory(ScriptCategory.Shop)
			.ForChapter(0)
			.ShowDialog(0x0104) // "Looking for supplies?"
			.OpenShop(ShopBurlandItem)
			.ShowDialog(0x0105) // "Good luck out there!"
			.End()
			.Build();
	}

	/// <summary>
	/// Build inn script.
	/// </summary>
	public static EventScript BuildInnScript() {
		return new EventScriptBuilder((ushort)(ScriptBase + 0x23))
			.WithName("Burland Inn")
			.WithCategory(ScriptCategory.Inn)
			.ForChapter(0)
			.ShowDialog(0x0106) // "Rest for the night?"
			.OpenInn(0x01, 8) // Inn ID 1, 8 gold
			.End()
			.Build();
	}

	/// <summary>
	/// Build church script.
	/// </summary>
	public static EventScript BuildChurchScript() {
		return new EventScriptBuilder((ushort)(ScriptBase + 0x24))
			.WithName("Burland Church")
			.WithCategory(ScriptCategory.Dialog)
			.ForChapter(0)
			.ShowDialog(0x0107) // "Blessings upon you."
			.OpenChurch()
			.End()
			.Build();
	}
}

namespace DW4Lib.Events;

/// <summary>
/// Chapter 2 (Alena) event scripts.
/// Contains all scripted events, dialogs, and triggers for Chapter 2.
/// </summary>
public static class Chapter2Events {
	// ============================================================
	// Script IDs
	// ============================================================

	/// <summary>Chapter 2 script ID base.</summary>
	public const ushort ScriptBase = 0x0200;

	/// <summary>Chapter intro script.</summary>
	public const ushort IntroScript = ScriptBase + 0x00;

	/// <summary>Alena sneaks out of castle.</summary>
	public const ushort SneakOut = ScriptBase + 0x01;

	/// <summary>Cristo and Brey join.</summary>
	public const ushort PartyJoins = ScriptBase + 0x02;

	/// <summary>Arrival at Tempe.</summary>
	public const ushort ArriveTempe = ScriptBase + 0x03;

	/// <summary>Meet the children at Tempe.</summary>
	public const ushort TempeChildren = ScriptBase + 0x04;

	/// <summary>Cave north of Tempe.</summary>
	public const ushort TempeCave = ScriptBase + 0x05;

	/// <summary>Bazaar announcement.</summary>
	public const ushort BazaarAnnounce = ScriptBase + 0x06;

	/// <summary>Arrival at Endor.</summary>
	public const ushort ArriveEndor = ScriptBase + 0x07;

	/// <summary>Tournament registration.</summary>
	public const ushort TournamentRegister = ScriptBase + 0x08;

	/// <summary>Tournament first round.</summary>
	public const ushort TournamentRound1 = ScriptBase + 0x09;

	/// <summary>Tournament second round.</summary>
	public const ushort TournamentRound2 = ScriptBase + 0x0A;

	/// <summary>Tournament semifinals.</summary>
	public const ushort TournamentSemifinal = ScriptBase + 0x0B;

	/// <summary>Tournament final.</summary>
	public const ushort TournamentFinal = ScriptBase + 0x0C;

	/// <summary>Necrosaro appears.</summary>
	public const ushort NecrosaroCutscene = ScriptBase + 0x0D;

	/// <summary>Return to Santeem.</summary>
	public const ushort ReturnSanteem = ScriptBase + 0x0E;

	/// <summary>Chapter complete.</summary>
	public const ushort ChapterComplete = ScriptBase + 0x0F;

	// ============================================================
	// Flag IDs
	// ============================================================

	/// <summary>Alena left castle.</summary>
	public const ushort FlagLeftCastle = 0x0101;

	/// <summary>Cristo and Brey joined.</summary>
	public const ushort FlagPartyJoined = 0x0102;

	/// <summary>Talked to Tempe children.</summary>
	public const ushort FlagTempeChildren = 0x0103;

	/// <summary>Cleared Tempe Cave.</summary>
	public const ushort FlagTempeCave = 0x0104;

	/// <summary>Heard bazaar announcement.</summary>
	public const ushort FlagBazaarAnnounce = 0x0105;

	/// <summary>Arrived at Endor.</summary>
	public const ushort FlagEndorArrived = 0x0106;

	/// <summary>Registered for tournament.</summary>
	public const ushort FlagTournamentReg = 0x0107;

	/// <summary>Won round 1.</summary>
	public const ushort FlagRound1Won = 0x0108;

	/// <summary>Won round 2.</summary>
	public const ushort FlagRound2Won = 0x0109;

	/// <summary>Won semifinal.</summary>
	public const ushort FlagSemifinalWon = 0x010A;

	/// <summary>Won tournament.</summary>
	public const ushort FlagTournamentWon = 0x010B;

	/// <summary>Necrosaro cutscene seen.</summary>
	public const ushort FlagNecrosaro = 0x010C;

	/// <summary>Returned to Santeem.</summary>
	public const ushort FlagReturnedSanteem = 0x010D;

	/// <summary>Chapter 2 complete.</summary>
	public const ushort FlagChapterComplete = 0x010E;

	// ============================================================
	// Battle IDs
	// ============================================================

	/// <summary>Tournament round 1 opponent.</summary>
	public const ushort BattleRound1 = 0x0011;

	/// <summary>Tournament round 2 opponent.</summary>
	public const ushort BattleRound2 = 0x0012;

	/// <summary>Tournament semifinal opponent.</summary>
	public const ushort BattleSemifinal = 0x0013;

	/// <summary>Tournament final opponent - Linguar.</summary>
	public const ushort BattleLinguar = 0x0014;

	// ============================================================
	// Music IDs
	// ============================================================

	/// <summary>Chapter 2 overworld theme.</summary>
	public const byte MusicOverworld = 0x20;

	/// <summary>Santeem Castle theme.</summary>
	public const byte MusicCastle = 0x21;

	/// <summary>Town theme.</summary>
	public const byte MusicTown = 0x22;

	/// <summary>Dungeon theme.</summary>
	public const byte MusicDungeon = 0x23;

	/// <summary>Tournament theme.</summary>
	public const byte MusicTournament = 0x24;

	/// <summary>Victory fanfare.</summary>
	public const byte MusicVictory = 0x25;

	/// <summary>Necrosaro theme.</summary>
	public const byte MusicNecrosaro = 0x26;

	// ============================================================
	// Shop IDs
	// ============================================================

	/// <summary>Santeem weapon shop.</summary>
	public const byte ShopSanteemWeapon = 0x11;

	/// <summary>Santeem armor shop.</summary>
	public const byte ShopSanteemArmor = 0x12;

	/// <summary>Santeem item shop.</summary>
	public const byte ShopSanteemItem = 0x13;

	/// <summary>Surene weapon shop.</summary>
	public const byte ShopSureneWeapon = 0x14;

	/// <summary>Surene item shop.</summary>
	public const byte ShopSureneItem = 0x15;

	/// <summary>Endor weapon shop.</summary>
	public const byte ShopEndorWeapon = 0x16;

	/// <summary>Endor armor shop.</summary>
	public const byte ShopEndorArmor = 0x17;

	/// <summary>Endor item shop.</summary>
	public const byte ShopEndorItem = 0x18;

	// ============================================================
	// Pre-built Scripts
	// ============================================================

	/// <summary>
	/// Get all Chapter 2 event scripts.
	/// </summary>
	public static EventScript[] GetAllScripts() => [
		BuildIntroScript(),
		BuildSneakOutScript(),
		BuildPartyJoinsScript(),
		BuildArriveTempeScript(),
		BuildTempeChildrenScript(),
		BuildTempeCaveScript(),
		BuildBazaarAnnounceScript(),
		BuildArriveEndorScript(),
		BuildTournamentRegisterScript(),
		BuildTournamentRound1Script(),
		BuildTournamentRound2Script(),
		BuildTournamentSemifinalScript(),
		BuildTournamentFinalScript(),
		BuildNecrosaroCutsceneScript(),
		BuildReturnSanteemScript(),
		BuildChapterCompleteScript(),
		// Service scripts
		BuildSanteemWeaponShopScript(),
		BuildSanteemArmorShopScript(),
		BuildSanteemItemShopScript(),
		BuildSanteemInnScript(),
		BuildSanteemChurchScript(),
		BuildEndorWeaponShopScript(),
		BuildEndorInnScript()
	];

	/// <summary>
	/// Chapter intro - Alena wants to leave castle.
	/// </summary>
	public static EventScript BuildIntroScript() {
		return new EventScriptBuilder(IntroScript)
			.WithName("Chapter 2 Intro")
			.WithCategory(ScriptCategory.Cutscene)
			.ForChapter(1)
			.FadeOut(8)
			.Wait(60)
			.PlayMusic(MusicCastle)
			.FadeIn(8)
			.Wait(30)
			.ShowDialog(0x0201) // "Princess Alena paces in her room..."
			.Wait(30)
			.ShowDialog(0x0202) // "I want to see the world!"
			.Wait(30)
			.ShowDialog(0x0203) // "Father won't let me leave..."
			.End()
			.Build();
	}

	/// <summary>
	/// Alena sneaks out of the castle.
	/// </summary>
	public static EventScript BuildSneakOutScript() {
		return new EventScriptBuilder(SneakOut)
			.WithName("Sneak Out")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(1)
			.CheckFlag(FlagLeftCastle, 0x0010)
			// First time
			.ShowDialog(0x0210) // "I'll sneak out through the secret passage!"
			.ShowDialog(0x0211) // "Father will understand eventually..."
			.SetFlag(FlagLeftCastle)
			.Warp(0x10, 5, 10) // Exit to Surene area
			.PlayMusic(MusicOverworld)
			.End()
			// Already left
			.ShowDialog(0x0212) // "I've already left the castle."
			.End()
			.Build();
	}

	/// <summary>
	/// Cristo and Brey join the party.
	/// </summary>
	public static EventScript BuildPartyJoinsScript() {
		return new EventScriptBuilder(PartyJoins)
			.WithName("Party Joins")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(1)
			.CheckFlag(FlagPartyJoined, 0x0020)
			// First encounter
			.ShowDialog(0x0220) // "Princess! Wait!"
			.ShowNpc(0x01) // Cristo
			.ShowNpc(0x04) // Brey
			.ShowDialog(0x0221) // "Cristo: We can't let you go alone!"
			.ShowDialog(0x0222) // "Brey: Indeed, someone must protect you."
			.ShowDialog(0x0223) // "Very well, you may come with me."
			.AddPartyMember(0x01) // Add Cristo
			.AddPartyMember(0x04) // Add Brey
			.SetFlag(FlagPartyJoined)
			.PlaySound(0x10) // Join sound
			.ShowDialog(0x0224) // "Cristo and Brey joined the party!"
			.End()
			// Already joined
			.ShowDialog(0x0225) // "Let's continue our adventure!"
			.End()
			.Build();
	}

	/// <summary>
	/// Arrival at Tempe village.
	/// </summary>
	public static EventScript BuildArriveTempeScript() {
		return new EventScriptBuilder(ArriveTempe)
			.WithName("Arrive Tempe")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(1)
			.PlayMusic(MusicTown)
			.ShowDialog(0x0230) // "This is Tempe village."
			.ShowDialog(0x0231) // "The villagers seem worried..."
			.End()
			.Build();
	}

	/// <summary>
	/// Talk to the children of Tempe.
	/// </summary>
	public static EventScript BuildTempeChildrenScript() {
		return new EventScriptBuilder(TempeChildren)
			.WithName("Tempe Children")
			.WithCategory(ScriptCategory.Dialog)
			.ForChapter(1)
			.CheckFlag(FlagTempeChildren, 0x0040)
			// First talk
			.ShowDialog(0x0240) // "Children: Please help us!"
			.ShowDialog(0x0241) // "A monster is in the cave north of here!"
			.ShowDialog(0x0242) // "It took our friend!"
			.SetFlag(FlagTempeChildren)
			.End()
			// After talking
			.ShowDialog(0x0243) // "Please save our friend!"
			.End()
			.Build();
	}

	/// <summary>
	/// Clear the cave north of Tempe.
	/// </summary>
	public static EventScript BuildTempeCaveScript() {
		return new EventScriptBuilder(TempeCave)
			.WithName("Tempe Cave")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(1)
			.CheckFlag(FlagTempeCave, 0x0050)
			// Boss encounter
			.PlayMusic(MusicDungeon)
			.ShowDialog(0x0250) // "You found the missing child!"
			.ShowDialog(0x0251) // "A monster blocks the way!"
			.StartBattle(0x0010) // Boss battle
			// After battle
			.ShowDialog(0x0252) // "The child is saved!"
			.SetFlag(FlagTempeCave)
			.GiveExp(200)
			.End()
			// Already cleared
			.ShowDialog(0x0253) // "The cave is empty now."
			.End()
			.Build();
	}

	/// <summary>
	/// Hear about the Endor tournament.
	/// </summary>
	public static EventScript BuildBazaarAnnounceScript() {
		return new EventScriptBuilder(BazaarAnnounce)
			.WithName("Bazaar Announce")
			.WithCategory(ScriptCategory.Dialog)
			.ForChapter(1)
			.CheckFlag(FlagBazaarAnnounce, 0x0060)
			// First hear
			.ShowDialog(0x0260) // "Announcement: The Endor Tournament!"
			.ShowDialog(0x0261) // "Warriors from all lands compete!"
			.ShowDialog(0x0262) // "The winner receives great fame!"
			.SetFlag(FlagBazaarAnnounce)
			.ShowDialog(0x0263) // "Alena: A tournament! I must enter!"
			.End()
			// Already heard
			.ShowDialog(0x0264) // "The tournament is in Endor."
			.End()
			.Build();
	}

	/// <summary>
	/// Arrival at Endor.
	/// </summary>
	public static EventScript BuildArriveEndorScript() {
		return new EventScriptBuilder(ArriveEndor)
			.WithName("Arrive Endor")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(1)
			.CheckFlag(FlagEndorArrived, 0x0070)
			// First arrival
			.PlayMusic(MusicTown)
			.ShowDialog(0x0270) // "This is Endor!"
			.ShowDialog(0x0271) // "The colosseum is here for the tournament."
			.SetFlag(FlagEndorArrived)
			.End()
			// Return visit
			.ShowDialog(0x0272) // "Welcome back to Endor!"
			.End()
			.Build();
	}

	/// <summary>
	/// Register for the tournament.
	/// </summary>
	public static EventScript BuildTournamentRegisterScript() {
		return new EventScriptBuilder(TournamentRegister)
			.WithName("Tournament Register")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(1)
			.CheckFlag(FlagTournamentReg, 0x0080)
			// Registration
			.ShowDialog(0x0280) // "Official: Welcome to the tournament!"
			.ShowDialog(0x0281) // "What is your name?"
			.ShowDialog(0x0282) // "Alena of Santeem? Very well!"
			.SetFlag(FlagTournamentReg)
			.ShowDialog(0x0283) // "You are registered. Good luck!"
			.End()
			// Already registered
			.ShowDialog(0x0284) // "The tournament begins soon!"
			.End()
			.Build();
	}

	/// <summary>
	/// Tournament round 1.
	/// </summary>
	public static EventScript BuildTournamentRound1Script() {
		return new EventScriptBuilder(TournamentRound1)
			.WithName("Tournament Round 1")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(1)
			.CheckFlag(FlagRound1Won, 0x0090)
			// First round battle
			.PlayMusic(MusicTournament)
			.ShowDialog(0x0290) // "Round 1! Alena vs Hun!"
			.RemovePartyMember(0x01) // Remove Cristo for solo fight
			.RemovePartyMember(0x04) // Remove Brey
			.StartBattle(BattleRound1)
			// Victory
			.ShowDialog(0x0291) // "Alena wins!"
			.SetFlag(FlagRound1Won)
			.AddPartyMember(0x01) // Re-add Cristo
			.AddPartyMember(0x04) // Re-add Brey
			.ShowDialog(0x0292) // "Proceed to round 2!"
			.End()
			// Already won
			.ShowDialog(0x0293) // "You won round 1!"
			.End()
			.Build();
	}

	/// <summary>
	/// Tournament round 2.
	/// </summary>
	public static EventScript BuildTournamentRound2Script() {
		return new EventScriptBuilder(TournamentRound2)
			.WithName("Tournament Round 2")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(1)
			.CheckFlag(FlagRound2Won, 0x00A0)
			// Round 2 battle
			.PlayMusic(MusicTournament)
			.ShowDialog(0x02A0) // "Round 2! Alena vs Roric!"
			.RemovePartyMember(0x01)
			.RemovePartyMember(0x04)
			.StartBattle(BattleRound2)
			// Victory
			.ShowDialog(0x02A1) // "Alena wins again!"
			.SetFlag(FlagRound2Won)
			.AddPartyMember(0x01)
			.AddPartyMember(0x04)
			.ShowDialog(0x02A2) // "Proceed to the semifinal!"
			.End()
			// Already won
			.ShowDialog(0x02A3) // "You won round 2!"
			.End()
			.Build();
	}

	/// <summary>
	/// Tournament semifinal.
	/// </summary>
	public static EventScript BuildTournamentSemifinalScript() {
		return new EventScriptBuilder(TournamentSemifinal)
			.WithName("Tournament Semifinal")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(1)
			.CheckFlag(FlagSemifinalWon, 0x00B0)
			// Semifinal battle
			.PlayMusic(MusicTournament)
			.ShowDialog(0x02B0) // "Semifinal! Alena vs Vivian!"
			.RemovePartyMember(0x01)
			.RemovePartyMember(0x04)
			.StartBattle(BattleSemifinal)
			// Victory
			.ShowDialog(0x02B1) // "Alena advances to the final!"
			.SetFlag(FlagSemifinalWon)
			.AddPartyMember(0x01)
			.AddPartyMember(0x04)
			.ShowDialog(0x02B2) // "The final match awaits!"
			.End()
			// Already won
			.ShowDialog(0x02B3) // "You won the semifinal!"
			.End()
			.Build();
	}

	/// <summary>
	/// Tournament final - vs Linguar.
	/// </summary>
	public static EventScript BuildTournamentFinalScript() {
		return new EventScriptBuilder(TournamentFinal)
			.WithName("Tournament Final")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(1)
			.CheckFlag(FlagTournamentWon, 0x00C0)
			// Final battle
			.PlayMusic(MusicTournament)
			.ShowDialog(0x02C0) // "The final! Alena vs Linguar!"
			.ShowDialog(0x02C1) // "This opponent is no ordinary warrior..."
			.RemovePartyMember(0x01)
			.RemovePartyMember(0x04)
			.StartBattle(BattleLinguar)
			// Victory
			.PlayMusic(MusicVictory)
			.ShowDialog(0x02C2) // "Alena is the champion!"
			.SetFlag(FlagTournamentWon)
			.AddPartyMember(0x01)
			.AddPartyMember(0x04)
			.GiveGold(1000) // Prize money
			.ShowDialog(0x02C3) // "You receive 1000 gold coins!"
			.End()
			// Already won
			.ShowDialog(0x02C4) // "You are the tournament champion!"
			.End()
			.Build();
	}

	/// <summary>
	/// Necrosaro appears at the tournament.
	/// </summary>
	public static EventScript BuildNecrosaroCutsceneScript() {
		return new EventScriptBuilder(NecrosaroCutscene)
			.WithName("Necrosaro Cutscene")
			.WithCategory(ScriptCategory.Cutscene)
			.ForChapter(1)
			.CheckFlag(FlagNecrosaro, 0x00D0)
			// Cutscene
			.FadeOut(8)
			.PlayMusic(MusicNecrosaro)
			.Wait(60)
			.FadeIn(8)
			.ShowDialog(0x02D0) // "A dark figure appears..."
			.ShowNpc(0xFF) // Necrosaro
			.ShowDialog(0x02D1) // "???: So this is the warrior..."
			.ShowDialog(0x02D2) // "Impressive... but futile."
			.ShowDialog(0x02D3) // "The figure vanishes!"
			.SetFlag(FlagNecrosaro)
			.PlayMusic(MusicTown)
			.ShowDialog(0x02D4) // "Who was that?!"
			.End()
			// Already seen
			.ShowDialog(0x02D5) // "That dark figure... who was he?"
			.End()
			.Build();
	}

	/// <summary>
	/// Return to Santeem Castle.
	/// </summary>
	public static EventScript BuildReturnSanteemScript() {
		return new EventScriptBuilder(ReturnSanteem)
			.WithName("Return Santeem")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(1)
			.CheckFlag(FlagReturnedSanteem, 0x00E0)
			// First return
			.PlayMusic(MusicCastle)
			.ShowDialog(0x02E0) // "Father! I've returned!"
			.ShowDialog(0x02E1) // "King: Alena! You're safe!"
			.ShowDialog(0x02E2) // "I won the tournament!"
			.ShowDialog(0x02E3) // "King: I'm proud of you, daughter."
			.SetFlag(FlagReturnedSanteem)
			.JumpSubroutine(ChapterComplete)
			.End()
			// Already returned
			.ShowDialog(0x02E4) // "Father is proud of me."
			.End()
			.Build();
	}

	/// <summary>
	/// Chapter 2 complete.
	/// </summary>
	public static EventScript BuildChapterCompleteScript() {
		return new EventScriptBuilder(ChapterComplete)
			.WithName("Chapter 2 Complete")
			.WithCategory(ScriptCategory.Cutscene)
			.ForChapter(1)
			.ShowDialog(0x02F0) // "And so Chapter 2 ends..."
			.ShowDialog(0x02F1) // "Alena's adventure continues..."
			.FadeOut(8)
			.Wait(120)
			.SetFlag(FlagChapterComplete)
			.SetChapter(2) // Advance to Chapter 3
			.End()
			.Build();
	}

	// ============================================================
	// Shop Scripts
	// ============================================================

	/// <summary>
	/// Santeem weapon shop script.
	/// </summary>
	public static EventScript BuildSanteemWeaponShopScript() {
		return new EventScriptBuilder((ushort)(ScriptBase + 0x30))
			.WithName("Santeem Weapon Shop")
			.WithCategory(ScriptCategory.Shop)
			.ForChapter(1)
			.ShowDialog(0x0300) // "Welcome to Santeem's weapon shop!"
			.OpenShop(ShopSanteemWeapon)
			.ShowDialog(0x0301) // "Come again!"
			.End()
			.Build();
	}

	/// <summary>
	/// Santeem armor shop script.
	/// </summary>
	public static EventScript BuildSanteemArmorShopScript() {
		return new EventScriptBuilder((ushort)(ScriptBase + 0x31))
			.WithName("Santeem Armor Shop")
			.WithCategory(ScriptCategory.Shop)
			.ForChapter(1)
			.ShowDialog(0x0302) // "Welcome! Need armor?"
			.OpenShop(ShopSanteemArmor)
			.ShowDialog(0x0303) // "Safe travels!"
			.End()
			.Build();
	}

	/// <summary>
	/// Santeem item shop script.
	/// </summary>
	public static EventScript BuildSanteemItemShopScript() {
		return new EventScriptBuilder((ushort)(ScriptBase + 0x32))
			.WithName("Santeem Item Shop")
			.WithCategory(ScriptCategory.Shop)
			.ForChapter(1)
			.ShowDialog(0x0304) // "Looking for supplies?"
			.OpenShop(ShopSanteemItem)
			.ShowDialog(0x0305) // "Good luck!"
			.End()
			.Build();
	}

	/// <summary>
	/// Santeem inn script.
	/// </summary>
	public static EventScript BuildSanteemInnScript() {
		return new EventScriptBuilder((ushort)(ScriptBase + 0x33))
			.WithName("Santeem Inn")
			.WithCategory(ScriptCategory.Inn)
			.ForChapter(1)
			.ShowDialog(0x0306) // "Rest for the night?"
			.OpenInn(0x11, 12) // Inn ID, 12 gold
			.End()
			.Build();
	}

	/// <summary>
	/// Santeem church script.
	/// </summary>
	public static EventScript BuildSanteemChurchScript() {
		return new EventScriptBuilder((ushort)(ScriptBase + 0x34))
			.WithName("Santeem Church")
			.WithCategory(ScriptCategory.Dialog)
			.ForChapter(1)
			.ShowDialog(0x0307) // "Blessings upon you."
			.OpenChurch()
			.End()
			.Build();
	}

	/// <summary>
	/// Endor weapon shop script.
	/// </summary>
	public static EventScript BuildEndorWeaponShopScript() {
		return new EventScriptBuilder((ushort)(ScriptBase + 0x35))
			.WithName("Endor Weapon Shop")
			.WithCategory(ScriptCategory.Shop)
			.ForChapter(1)
			.ShowDialog(0x0308) // "Welcome to Endor's finest weapons!"
			.OpenShop(ShopEndorWeapon)
			.ShowDialog(0x0309) // "May you fight well!"
			.End()
			.Build();
	}

	/// <summary>
	/// Endor inn script.
	/// </summary>
	public static EventScript BuildEndorInnScript() {
		return new EventScriptBuilder((ushort)(ScriptBase + 0x36))
			.WithName("Endor Inn")
			.WithCategory(ScriptCategory.Inn)
			.ForChapter(1)
			.ShowDialog(0x030A) // "Welcome, traveler!"
			.OpenInn(0x12, 20) // Inn ID, 20 gold
			.End()
			.Build();
	}
}

using DW4Lib.Maps;

namespace DW4Lib.Events;

/// <summary>
/// Chapter 4 (Meena and Maya) event scripts.
/// Contains all scripted events, dialogs, and triggers for Chapter 4.
/// The sisters seek revenge for their father Mahabala's murder by Balzack.
/// </summary>
public static class Chapter4Events {
	// ============================================================
	// Script IDs
	// ============================================================

	/// <summary>Chapter 4 script ID base.</summary>
	public const ushort ScriptBase = 0x0400;

	/// <summary>Chapter intro - father's murder flashback.</summary>
	public const ushort IntroScript = ScriptBase + 0x00;

	/// <summary>Morning in Monbaraba theater.</summary>
	public const ushort MorningTheater = ScriptBase + 0x01;

	/// <summary>First dance performance.</summary>
	public const ushort DancePerformance = ScriptBase + 0x02;

	/// <summary>Fortune telling tutorial.</summary>
	public const ushort FortuneTelling = ScriptBase + 0x03;

	/// <summary>Hear rumors about Balzack.</summary>
	public const ushort BalzackRumors = ScriptBase + 0x04;

	/// <summary>Meet Orin the informant.</summary>
	public const ushort MeetOrin = ScriptBase + 0x05;

	/// <summary>Cave of Monbaraba entrance.</summary>
	public const ushort CaveMonbaraba = ScriptBase + 0x06;

	/// <summary>Find Sphere of Silence.</summary>
	public const ushort FindSphereOfSilence = ScriptBase + 0x07;

	/// <summary>Arrive in Haville.</summary>
	public const ushort ArriveHaville = ScriptBase + 0x08;

	/// <summary>Hear about Alchemy researcher.</summary>
	public const ushort AlchemyRumors = ScriptBase + 0x09;

	/// <summary>Mine entrance.</summary>
	public const ushort MineEntrance = ScriptBase + 0x0A;

	/// <summary>Deep mine discovery.</summary>
	public const ushort MineDeep = ScriptBase + 0x0B;

	/// <summary>Find Gunpowder Jar.</summary>
	public const ushort FindGunpowder = ScriptBase + 0x0C;

	/// <summary>Travel to Kievs.</summary>
	public const ushort TravelKievs = ScriptBase + 0x0D;

	/// <summary>Kievs Castle entry.</summary>
	public const ushort KievsCastle = ScriptBase + 0x0E;

	/// <summary>Balzack encounter.</summary>
	public const ushort BalzackEncounter = ScriptBase + 0x0F;

	/// <summary>Balzack battle.</summary>
	public const ushort BalzackBattle = ScriptBase + 0x10;

	/// <summary>Escape from Kievs.</summary>
	public const ushort EscapeKievs = ScriptBase + 0x11;

	/// <summary>Arrive in Endor.</summary>
	public const ushort ArriveEndor = ScriptBase + 0x12;

	/// <summary>Endor Tournament preparation.</summary>
	public const ushort TournamentPrep = ScriptBase + 0x13;

	/// <summary>Chapter complete transition.</summary>
	public const ushort ChapterComplete = ScriptBase + 0x14;

	// ============================================================
	// Service Script IDs
	// ============================================================

	/// <summary>Monbaraba Inn service.</summary>
	public const ushort MonbarabaInn = ScriptBase + 0x20;

	/// <summary>Monbaraba Item shop.</summary>
	public const ushort MonbarabaItemShop = ScriptBase + 0x21;

	/// <summary>Monbaraba Weapon shop.</summary>
	public const ushort MonbarabaWeaponShop = ScriptBase + 0x22;

	/// <summary>Monbaraba Church.</summary>
	public const ushort MonbarabaChurch = ScriptBase + 0x23;

	/// <summary>Haville Inn.</summary>
	public const ushort HavilleInn = ScriptBase + 0x24;

	/// <summary>Haville Item shop.</summary>
	public const ushort HavilleItemShop = ScriptBase + 0x25;

	/// <summary>Kievs Inn.</summary>
	public const ushort KievsInn = ScriptBase + 0x26;

	/// <summary>Kievs Item shop.</summary>
	public const ushort KievsItemShop = ScriptBase + 0x27;

	/// <summary>Theater manager dialog.</summary>
	public const ushort TheaterManager = ScriptBase + 0x28;

	/// <summary>Orin NPC dialog.</summary>
	public const ushort OrinDialog = ScriptBase + 0x29;

	// ============================================================
	// Flag IDs
	// ============================================================

	/// <summary>Saw intro flashback.</summary>
	public const ushort FlagIntro = 0x0401;

	/// <summary>Performed first dance.</summary>
	public const ushort FlagDancePerformed = 0x0402;

	/// <summary>Did fortune telling.</summary>
	public const ushort FlagFortuneTold = 0x0403;

	/// <summary>Heard Balzack rumors.</summary>
	public const ushort FlagBalzackRumors = 0x0404;

	/// <summary>Met Orin.</summary>
	public const ushort FlagMetOrin = 0x0405;

	/// <summary>Entered Monbaraba Cave.</summary>
	public const ushort FlagCaveMonbaraba = 0x0406;

	/// <summary>Found Sphere of Silence.</summary>
	public const ushort FlagSphereOfSilence = 0x0407;

	/// <summary>Arrived in Haville.</summary>
	public const ushort FlagHaville = 0x0408;

	/// <summary>Heard about alchemy.</summary>
	public const ushort FlagAlchemyRumors = 0x0409;

	/// <summary>Entered mine.</summary>
	public const ushort FlagMineEntered = 0x040A;

	/// <summary>Found gunpowder.</summary>
	public const ushort FlagGunpowder = 0x040B;

	/// <summary>Arrived in Kievs.</summary>
	public const ushort FlagKievs = 0x040C;

	/// <summary>Entered Kievs Castle.</summary>
	public const ushort FlagKievsCastle = 0x040D;

	/// <summary>Encountered Balzack.</summary>
	public const ushort FlagBalzackEncounter = 0x040E;

	/// <summary>Defeated/Escaped Balzack.</summary>
	public const ushort FlagBalzackBattle = 0x040F;

	/// <summary>Escaped Kievs.</summary>
	public const ushort FlagEscapedKievs = 0x0410;

	/// <summary>Arrived in Endor.</summary>
	public const ushort FlagEndor = 0x0411;

	/// <summary>Chapter complete.</summary>
	public const ushort FlagChapterComplete = 0x0412;

	// ============================================================
	// Battle IDs
	// ============================================================

	/// <summary>Balzack boss battle (first form - unwinnable).</summary>
	public const ushort BattleBalzack = 0x0401;

	// ============================================================
	// Music IDs
	// ============================================================

	/// <summary>Chapter 4 overworld theme.</summary>
	public const byte MusicOverworld = 0x40;

	/// <summary>Monbaraba town theme.</summary>
	public const byte MusicMonbaraba = 0x41;

	/// <summary>Theater dance music.</summary>
	public const byte MusicDance = 0x42;

	/// <summary>Fortune telling music.</summary>
	public const byte MusicFortune = 0x43;

	/// <summary>Dungeon theme.</summary>
	public const byte MusicDungeon = 0x44;

	/// <summary>Kievs Castle theme.</summary>
	public const byte MusicKievsCastle = 0x45;

	/// <summary>Balzack boss theme.</summary>
	public const byte MusicBalzack = 0x46;

	/// <summary>Sisters' sad theme.</summary>
	public const byte MusicSadness = 0x47;

	// ============================================================
	// Item IDs
	// ============================================================

	/// <summary>Sphere of Silence.</summary>
	public const byte ItemSphereOfSilence = 0x70;

	/// <summary>Gunpowder Jar.</summary>
	public const byte ItemGunpowder = 0x71;

	/// <summary>Father's keepsake.</summary>
	public const byte ItemFatherKeepsake = 0x72;

	/// <summary>Theater earnings (gold).</summary>
	public const byte ItemTheaterGold = 100;

	/// <summary>Fortune telling earnings (gold).</summary>
	public const byte ItemFortuneGold = 50;

	// ============================================================
	// Character IDs
	// ============================================================

	/// <summary>Meena (party member).</summary>
	public const byte CharacterMeena = 0x05;

	/// <summary>Maya (party member).</summary>
	public const byte CharacterMaya = 0x06;

	/// <summary>Orin (NPC ally).</summary>
	public const byte CharacterOrin = 0x40;

	/// <summary>Balzack (boss).</summary>
	public const byte CharacterBalzack = 0xF4;

	/// <summary>Mahabala (father, deceased).</summary>
	public const byte CharacterMahabala = 0x41;

	// ============================================================
	// Story Scripts
	// ============================================================

	/// <summary>
	/// Chapter 4 intro - father's murder flashback.
	/// </summary>
	public static EventScript BuildIntroScript() {
		return new EventScriptBuilder(IntroScript)
			.WithName("Chapter 4 Intro")
			.WithCategory(ScriptCategory.Cutscene)
			.ForChapter(3)
			.FadeOut()
			.PlayMusic(MusicSadness)
			// Flashback scene
			.ShowDialog(0x0500) // "Narrator: In the kingdom of Kievs..."
			.ShowDialog(0x0501) // "Narrator: The great alchemist Mahabala lived peacefully..."
			.ShowDialog(0x0502) // "Narrator: Until the day his apprentice Balzack betrayed him..."
			.ShowDialog(0x0503) // "Balzack: Give me the Secret of Evolution!"
			.ShowDialog(0x0504) // "Mahabala: Never! You would misuse its power!"
			.ShowDialog(0x0505) // "Narrator: Mahabala was struck down before his daughters' eyes..."
			.ShowDialog(0x0506) // "Meena: Father! No!"
			.ShowDialog(0x0507) // "Maya: We will avenge you, father!"
			// Present day
			.FadeIn()
			.PlayMusic(MusicMonbaraba)
			.ShowDialog(0x0508) // "Narrator: Years later, in the entertainment town of Monbaraba..."
			.ShowDialog(0x0509) // "Meena: Sister, are you ready for today's performance?"
			.ShowDialog(0x050A) // "Maya: Yes. But I haven't forgotten our vow."
			.SetFlag(FlagIntro)
			.End()
			.Build();
	}

	/// <summary>
	/// Morning at the theater - sisters prepare for work.
	/// </summary>
	public static EventScript BuildMorningTheaterScript() {
		return new EventScriptBuilder(MorningTheater)
			.WithName("Morning Theater")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(3)
			.CheckFlag(FlagDancePerformed, 0x0020)
			// First day
			.ShowDialog(0x0510) // "Theater Manager: Ah, Meena and Maya!"
			.ShowDialog(0x0511) // "Theater Manager: The audience awaits your dance!"
			.ShowDialog(0x0512) // "Maya: Don't worry, we'll give them a show!"
			.ShowDialog(0x0513) // "Meena: I'll read fortunes afterward."
			.End()
			// Already performed
			.ShowDialog(0x0514) // "Theater Manager: Great work today, ladies!"
			.End()
			.Build();
	}

	/// <summary>
	/// Maya's dance performance earns gold.
	/// </summary>
	public static EventScript BuildDancePerformanceScript() {
		return new EventScriptBuilder(DancePerformance)
			.WithName("Dance Performance")
			.WithCategory(ScriptCategory.Cutscene)
			.ForChapter(3)
			.CheckFlag(FlagDancePerformed, 0x0030)
			// Performance
			.FadeOut()
			.PlayMusic(MusicDance)
			.Wait(60) // Dance animation
			.FadeIn()
			.ShowDialog(0x0520) // "Maya performs a captivating dance!"
			.ShowDialog(0x0521) // "Crowd: Bravo! Magnificent!"
			.ShowDialog(0x0522) // "Maya: Thank you all!"
			.GiveGold(ItemTheaterGold)
			.ShowDialog(0x0523) // "Received 100 gold pieces!"
			.SetFlag(FlagDancePerformed)
			.PlayMusic(MusicMonbaraba)
			.End()
			// Daily repeat
			.GiveGold(ItemTheaterGold / 2)
			.ShowDialog(0x0524) // "Maya performs her dance. Received 50 gold!"
			.End()
			.Build();
	}

	/// <summary>
	/// Meena's fortune telling service.
	/// </summary>
	public static EventScript BuildFortuneTellingScript() {
		return new EventScriptBuilder(FortuneTelling)
			.WithName("Fortune Telling")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(3)
			.CheckFlag(FlagFortuneTold, 0x0030)
			// First reading
			.PlayMusic(MusicFortune)
			.ShowDialog(0x0530) // "Customer: Can you tell my fortune?"
			.ShowDialog(0x0531) // "Meena: I see... great change in your future..."
			.ShowDialog(0x0532) // "Customer: Amazing! Here's your payment!"
			.GiveGold(ItemFortuneGold)
			.ShowDialog(0x0533) // "Received 50 gold pieces!"
			.SetFlag(FlagFortuneTold)
			.PlayMusic(MusicMonbaraba)
			.End()
			// Repeat readings
			.GiveGold(ItemFortuneGold / 2)
			.ShowDialog(0x0534) // "Meena reads fortunes. Received 25 gold!"
			.End()
			.Build();
	}

	/// <summary>
	/// Hear rumors about Balzack's whereabouts.
	/// </summary>
	public static EventScript BuildBalzackRumorsScript() {
		return new EventScriptBuilder(BalzackRumors)
			.WithName("Balzack Rumors")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(3)
			.CheckFlag(FlagBalzackRumors, 0x0020)
			// First time
			.ShowDialog(0x0540) // "Man: Have you heard? The kingdom of Kievs..."
			.ShowDialog(0x0541) // "Man: They say a sorcerer named Balzack rules there now!"
			.ShowDialog(0x0542) // "Maya: Balzack! It's him!"
			.ShowDialog(0x0543) // "Meena: We must go to Kievs!"
			.SetFlag(FlagBalzackRumors)
			.End()
			// After
			.ShowDialog(0x0544) // "Man: Kievs is far to the east..."
			.End()
			.Build();
	}

	/// <summary>
	/// Meet Orin who provides information.
	/// </summary>
	public static EventScript BuildMeetOrinScript() {
		return new EventScriptBuilder(MeetOrin)
			.WithName("Meet Orin")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(3)
			.CheckFlag(FlagMetOrin, 0x0020)
			// First meeting
			.ShowDialog(0x0550) // "Orin: You're Mahabala's daughters, aren't you?"
			.ShowDialog(0x0551) // "Meena: How do you know our father?"
			.ShowDialog(0x0552) // "Orin: He was a great man. I'll help you."
			.ShowDialog(0x0553) // "Orin: There's a cave west of town with a useful item."
			.ShowDialog(0x0554) // "Orin: The Sphere of Silence can neutralize magic!"
			.SetFlag(FlagMetOrin)
			.End()
			// After
			.ShowDialog(0x0555) // "Orin: Check the cave west of Monbaraba."
			.End()
			.Build();
	}

	/// <summary>
	/// Entering the Cave of Monbaraba.
	/// </summary>
	public static EventScript BuildCaveMonbarabaScript() {
		return new EventScriptBuilder(CaveMonbaraba)
			.WithName("Cave Monbaraba")
			.WithCategory(ScriptCategory.Trigger)
			.ForChapter(3)
			.CheckFlag(FlagCaveMonbaraba, 0x0010)
			// First entry
			.ShowDialog(0x0560) // "Maya: This must be the cave Orin mentioned!"
			.ShowDialog(0x0561) // "Meena: Be careful, I sense monsters inside..."
			.SetFlag(FlagCaveMonbaraba)
			.End()
			// Return
			.End()
			.Build();
	}

	/// <summary>
	/// Find the Sphere of Silence.
	/// </summary>
	public static EventScript BuildFindSphereOfSilenceScript() {
		return new EventScriptBuilder(FindSphereOfSilence)
			.WithName("Find Sphere of Silence")
			.WithCategory(ScriptCategory.Item)
			.ForChapter(3)
			.CheckFlag(FlagSphereOfSilence, 0x0020)
			// First time
			.ShowDialog(0x0570) // "Found a strange orb in the chest!"
			.GiveItem(ItemSphereOfSilence)
			.ShowDialog(0x0571) // "Obtained Sphere of Silence!"
			.ShowDialog(0x0572) // "Meena: This will help against Balzack's magic!"
			.SetFlag(FlagSphereOfSilence)
			.End()
			// Already obtained
			.ShowDialog(0x0573) // "The chest is empty."
			.End()
			.Build();
	}

	/// <summary>
	/// Arrive in Haville mining town.
	/// </summary>
	public static EventScript BuildArriveHavilleScript() {
		return new EventScriptBuilder(ArriveHaville)
			.WithName("Arrive Haville")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(3)
			.CheckFlag(FlagHaville, 0x0010)
			// First visit
			.ShowDialog(0x0580) // "Maya: This is Haville, the mining town."
			.ShowDialog(0x0581) // "Meena: The mine might have useful items..."
			.SetFlag(FlagHaville)
			.End()
			// Return
			.End()
			.Build();
	}

	/// <summary>
	/// Hear about alchemy research in the mines.
	/// </summary>
	public static EventScript BuildAlchemyRumorsScript() {
		return new EventScriptBuilder(AlchemyRumors)
			.WithName("Alchemy Rumors")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(3)
			.CheckFlag(FlagAlchemyRumors, 0x0020)
			// First time
			.ShowDialog(0x0590) // "Miner: Our grandfather was an alchemist too..."
			.ShowDialog(0x0591) // "Miner: He left something deep in the mine."
			.ShowDialog(0x0592) // "Miner: Gunpowder, I think he called it."
			.ShowDialog(0x0593) // "Maya: Gunpowder? That could be useful!"
			.SetFlag(FlagAlchemyRumors)
			.End()
			// After
			.ShowDialog(0x0594) // "Miner: The deep mine is dangerous..."
			.End()
			.Build();
	}

	/// <summary>
	/// Mine entrance event.
	/// </summary>
	public static EventScript BuildMineEntranceScript() {
		return new EventScriptBuilder(MineEntrance)
			.WithName("Mine Entrance")
			.WithCategory(ScriptCategory.Trigger)
			.ForChapter(3)
			.CheckFlag(FlagMineEntered, 0x0010)
			// First entry
			.ShowDialog(0x05A0) // "The mine shaft descends into darkness..."
			.ShowDialog(0x05A1) // "Meena: Stay close, sister."
			.SetFlag(FlagMineEntered)
			.End()
			// Return
			.End()
			.Build();
	}

	/// <summary>
	/// Deep mine exploration.
	/// </summary>
	public static EventScript BuildMineDeepScript() {
		return new EventScriptBuilder(MineDeep)
			.WithName("Mine Deep")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(3)
			.ShowDialog(0x05B0) // "The air grows thick with dust..."
			.ShowDialog(0x05B1) // "Ancient mining equipment lies abandoned here."
			.End()
			.Build();
	}

	/// <summary>
	/// Find the Gunpowder Jar.
	/// </summary>
	public static EventScript BuildFindGunpowderScript() {
		return new EventScriptBuilder(FindGunpowder)
			.WithName("Find Gunpowder")
			.WithCategory(ScriptCategory.Item)
			.ForChapter(3)
			.CheckFlag(FlagGunpowder, 0x0020)
			// First time
			.ShowDialog(0x05C0) // "Found a jar of strange powder!"
			.GiveItem(ItemGunpowder)
			.ShowDialog(0x05C1) // "Obtained Gunpowder Jar!"
			.ShowDialog(0x05C2) // "Maya: This is the gunpowder! Handle with care!"
			.SetFlag(FlagGunpowder)
			.End()
			// Already obtained
			.ShowDialog(0x05C3) // "The area is empty."
			.End()
			.Build();
	}

	/// <summary>
	/// Travel to Kievs.
	/// </summary>
	public static EventScript BuildTravelKievsScript() {
		return new EventScriptBuilder(TravelKievs)
			.WithName("Travel Kievs")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(3)
			.CheckFlag(FlagKievs, 0x0010)
			// First arrival
			.ShowDialog(0x05D0) // "Meena: This is Kievs... our homeland."
			.ShowDialog(0x05D1) // "Maya: And where Balzack awaits."
			.ShowDialog(0x05D2) // "Meena: Let's find a way into the castle."
			.SetFlag(FlagKievs)
			.End()
			// Return
			.End()
			.Build();
	}

	/// <summary>
	/// Kievs Castle entry.
	/// </summary>
	public static EventScript BuildKievsCastleScript() {
		return new EventScriptBuilder(KievsCastle)
			.WithName("Kievs Castle")
			.WithCategory(ScriptCategory.Trigger)
			.ForChapter(3)
			.CheckFlag(FlagKievsCastle, 0x0010)
			// First entry
			.PlayMusic(MusicKievsCastle)
			.ShowDialog(0x05E0) // "Guard: Halt! Who goes there?"
			.ShowDialog(0x05E1) // "Maya: We're traveling performers!"
			.ShowDialog(0x05E2) // "Guard: Lord Balzack enjoys entertainment... Enter."
			.SetFlag(FlagKievsCastle)
			.End()
			// Return
			.End()
			.Build();
	}

	/// <summary>
	/// Confrontation with Balzack.
	/// </summary>
	public static EventScript BuildBalzackEncounterScript() {
		return new EventScriptBuilder(BalzackEncounter)
			.WithName("Balzack Encounter")
			.WithCategory(ScriptCategory.Cutscene)
			.ForChapter(3)
			.CheckFlag(FlagBalzackEncounter, 0x0040)
			// First encounter
			.ShowDialog(0x05F0) // "Balzack: Ah, performers! Dance for me!"
			.ShowDialog(0x05F1) // "Maya: Balzack! You murdered our father!"
			.ShowDialog(0x05F2) // "Balzack: Mahabala's daughters? Impossible!"
			.ShowDialog(0x05F3) // "Meena: We've come to avenge him!"
			.ShowDialog(0x05F4) // "Balzack: Fools! I've evolved beyond human limits!"
			.ShowDialog(0x05F5) // "Balzack: You cannot defeat me!"
			.SetFlag(FlagBalzackEncounter)
			.End()
			// After
			.ShowDialog(0x05F6) // "Balzack: Guards! Seize them!"
			.End()
			.Build();
	}

	/// <summary>
	/// Balzack battle (scripted to be unwinnable).
	/// </summary>
	public static EventScript BuildBalzackBattleScript() {
		return new EventScriptBuilder(BalzackBattle)
			.WithName("Balzack Battle")
			.WithCategory(ScriptCategory.Battle)
			.ForChapter(3)
			.CheckFlag(FlagBalzackBattle, 0x0030)
			// Battle initiation
			.PlayMusic(MusicBalzack)
			.StartBattle(BattleBalzack)
			// After battle (scripted loss)
			.FadeOut()
			.ShowDialog(0x0600) // "Balzack: Pathetic! You're no match for me!"
			.ShowDialog(0x0601) // "Maya: We... we can't win like this..."
			.ShowDialog(0x0602) // "Meena: We need to escape and find help!"
			.SetFlag(FlagBalzackBattle)
			.End()
			// Already fought
			.ShowDialog(0x0603) // "Balzack: You again? Guards!"
			.End()
			.Build();
	}

	/// <summary>
	/// Escape from Kievs Castle.
	/// </summary>
	public static EventScript BuildEscapeKievsScript() {
		return new EventScriptBuilder(EscapeKievs)
			.WithName("Escape Kievs")
			.WithCategory(ScriptCategory.Cutscene)
			.ForChapter(3)
			.CheckFlag(FlagEscapedKievs, 0x0030)
			// Escape sequence
			.ShowDialog(0x0610) // "Orin appears from the shadows!"
			.ShowDialog(0x0611) // "Orin: Quick! This way!"
			.ShowDialog(0x0612) // "Orin leads you through a secret passage."
			.FadeOut()
			.PlayMusic(MusicOverworld)
			.Warp(Chapter4Maps.MapChapter4Overworld, 10, 15) // Outside Kievs on overworld
			.FadeIn()
			.ShowDialog(0x0613) // "Orin: Head to Endor. Seek the Hero!"
			.ShowDialog(0x0614) // "Meena: The Hero? The one from the prophecy?"
			.ShowDialog(0x0615) // "Orin: Yes. Only together can you defeat Balzack."
			.SetFlag(FlagEscapedKievs)
			.End()
			// Already escaped
			.ShowDialog(0x0616) // "Cannot return to Kievs Castle now."
			.End()
			.Build();
	}

	/// <summary>
	/// Arrive in Endor.
	/// </summary>
	public static EventScript BuildArriveEndorScript() {
		return new EventScriptBuilder(ArriveEndor)
			.WithName("Arrive Endor")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(3)
			.CheckFlag(FlagEndor, 0x0010)
			// First arrival
			.ShowDialog(0x0620) // "Maya: Endor! The city of tournaments!"
			.ShowDialog(0x0621) // "Meena: Maybe we can find the Hero here..."
			.ShowDialog(0x0622) // "Maya: Let's ask around about the tournament."
			.SetFlag(FlagEndor)
			.End()
			// Return
			.End()
			.Build();
	}

	/// <summary>
	/// Tournament preparation and chapter end.
	/// </summary>
	public static EventScript BuildTournamentPrepScript() {
		return new EventScriptBuilder(TournamentPrep)
			.WithName("Tournament Prep")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(3)
			.ShowDialog(0x0630) // "Tournament Official: The tournament draws many warriors!"
			.ShowDialog(0x0631) // "Tournament Official: Perhaps the Hero will appear..."
			.ShowDialog(0x0632) // "Meena: We'll wait and watch."
			.ShowDialog(0x0633) // "Maya: And when we find help, we'll return to Kievs!"
			.End()
			.Build();
	}

	/// <summary>
	/// Chapter 4 complete - transition to Chapter 5.
	/// </summary>
	public static EventScript BuildChapterCompleteScript() {
		return new EventScriptBuilder(ChapterComplete)
			.WithName("Chapter 4 Complete")
			.WithCategory(ScriptCategory.System)
			.ForChapter(3)
			.ShowDialog(0x0640) // "Narrator: The sisters wait in Endor..."
			.ShowDialog(0x0641) // "Narrator: Unaware that fate has already set the Hero's path..."
			.ShowDialog(0x0642) // "Narrator: Now begins the final chapter..."
			.SetFlag(FlagChapterComplete)
			.SetChapter(4) // Move to Chapter 5 (Hero's chapter)
			.End()
			.Build();
	}

	// ============================================================
	// Service Scripts
	// ============================================================

	/// <summary>
	/// Monbaraba Inn service.
	/// </summary>
	public static EventScript BuildMonbarabaInnScript() {
		return new EventScriptBuilder(MonbarabaInn)
			.WithName("Monbaraba Inn")
			.WithCategory(ScriptCategory.Inn)
			.ForChapter(3)
			.ShowDialog(0x0700) // "Innkeeper: Welcome! Rest for 20 gold?"
			.OpenInn(0x20, 20)
			.Return()
			.Build();
	}

	/// <summary>
	/// Monbaraba Item shop service.
	/// </summary>
	public static EventScript BuildMonbarabaItemShopScript() {
		return new EventScriptBuilder(MonbarabaItemShop)
			.WithName("Monbaraba Item Shop")
			.WithCategory(ScriptCategory.Shop)
			.ForChapter(3)
			.ShowDialog(0x0710) // "Shopkeeper: What would you like?"
			.OpenShop(0x40) // Monbaraba item inventory
			.Return()
			.Build();
	}

	/// <summary>
	/// Monbaraba Weapon shop service.
	/// </summary>
	public static EventScript BuildMonbarabaWeaponShopScript() {
		return new EventScriptBuilder(MonbarabaWeaponShop)
			.WithName("Monbaraba Weapon Shop")
			.WithCategory(ScriptCategory.Shop)
			.ForChapter(3)
			.ShowDialog(0x0720) // "Armorer: Looking for weapons or armor?"
			.OpenShop(0x41) // Monbaraba weapon inventory
			.Return()
			.Build();
	}

	/// <summary>
	/// Monbaraba Church service.
	/// </summary>
	public static EventScript BuildMonbarabaChurchScript() {
		return new EventScriptBuilder(MonbarabaChurch)
			.WithName("Monbaraba Church")
			.WithCategory(ScriptCategory.NPC)
			.ForChapter(3)
			.ShowDialog(0x0730) // "Priest: May the Goddess bless you."
			.OpenChurch()
			.Return()
			.Build();
	}

	/// <summary>
	/// Haville Inn service.
	/// </summary>
	public static EventScript BuildHavilleInnScript() {
		return new EventScriptBuilder(HavilleInn)
			.WithName("Haville Inn")
			.WithCategory(ScriptCategory.Inn)
			.ForChapter(3)
			.ShowDialog(0x0740) // "Innkeeper: Rest for 15 gold?"
			.OpenInn(0x21, 15)
			.Return()
			.Build();
	}

	/// <summary>
	/// Haville Item shop service.
	/// </summary>
	public static EventScript BuildHavilleItemShopScript() {
		return new EventScriptBuilder(HavilleItemShop)
			.WithName("Haville Item Shop")
			.WithCategory(ScriptCategory.Shop)
			.ForChapter(3)
			.ShowDialog(0x0750) // "Shopkeeper: Mining supplies here!"
			.OpenShop(0x42) // Haville item inventory
			.Return()
			.Build();
	}

	/// <summary>
	/// Kievs Inn service.
	/// </summary>
	public static EventScript BuildKievsInnScript() {
		return new EventScriptBuilder(KievsInn)
			.WithName("Kievs Inn")
			.WithCategory(ScriptCategory.Inn)
			.ForChapter(3)
			.ShowDialog(0x0760) // "Innkeeper: You look tired... 25 gold."
			.OpenInn(0x22, 25)
			.Return()
			.Build();
	}

	/// <summary>
	/// Kievs Item shop service.
	/// </summary>
	public static EventScript BuildKievsItemShopScript() {
		return new EventScriptBuilder(KievsItemShop)
			.WithName("Kievs Item Shop")
			.WithCategory(ScriptCategory.Shop)
			.ForChapter(3)
			.ShowDialog(0x0770) // "Shopkeeper: Times are hard under Balzack..."
			.OpenShop(0x43) // Kievs item inventory
			.Return()
			.Build();
	}

	/// <summary>
	/// Theater manager dialog.
	/// </summary>
	public static EventScript BuildTheaterManagerScript() {
		return new EventScriptBuilder(TheaterManager)
			.WithName("Theater Manager")
			.WithCategory(ScriptCategory.NPC)
			.ForChapter(3)
			.CheckFlag(FlagDancePerformed, 0x0010)
			// Before performance
			.ShowDialog(0x0780) // "Manager: The crowd is waiting, ladies!"
			.End()
			// After
			.ShowDialog(0x0781) // "Manager: Excellent work today!"
			.End()
			.Build();
	}

	/// <summary>
	/// Orin dialog.
	/// </summary>
	public static EventScript BuildOrinDialogScript() {
		return new EventScriptBuilder(OrinDialog)
			.WithName("Orin Dialog")
			.WithCategory(ScriptCategory.NPC)
			.ForChapter(3)
			.CheckFlag(FlagEscapedKievs, 0x0020)
			// After escape
			.ShowDialog(0x0790) // "Orin: Find the Hero in Endor. He's your only hope."
			.End()
			// Before meeting
			.ShowDialog(0x0791) // "Orin: I knew your father. He was a great man."
			.End()
			.Build();
	}

	// ============================================================
	// Script Collections
	// ============================================================

	/// <summary>
	/// Get all Chapter 4 scripts.
	/// </summary>
	public static List<EventScript> GetAllScripts() {
		return [
			// Story scripts
			BuildIntroScript(),
			BuildMorningTheaterScript(),
			BuildDancePerformanceScript(),
			BuildFortuneTellingScript(),
			BuildBalzackRumorsScript(),
			BuildMeetOrinScript(),
			BuildCaveMonbarabaScript(),
			BuildFindSphereOfSilenceScript(),
			BuildArriveHavilleScript(),
			BuildAlchemyRumorsScript(),
			BuildMineEntranceScript(),
			BuildMineDeepScript(),
			BuildFindGunpowderScript(),
			BuildTravelKievsScript(),
			BuildKievsCastleScript(),
			BuildBalzackEncounterScript(),
			BuildBalzackBattleScript(),
			BuildEscapeKievsScript(),
			BuildArriveEndorScript(),
			BuildTournamentPrepScript(),
			BuildChapterCompleteScript(),
			// Service scripts
			BuildMonbarabaInnScript(),
			BuildMonbarabaItemShopScript(),
			BuildMonbarabaWeaponShopScript(),
			BuildMonbarabaChurchScript(),
			BuildHavilleInnScript(),
			BuildHavilleItemShopScript(),
			BuildKievsInnScript(),
			BuildKievsItemShopScript(),
			BuildTheaterManagerScript(),
			BuildOrinDialogScript(),
		];
	}

	/// <summary>
	/// Get all story scripts (non-service).
	/// </summary>
	public static List<EventScript> GetStoryScripts() {
		return GetAllScripts()
			.Where(s => s.Category != ScriptCategory.Inn &&
						s.Category != ScriptCategory.Shop &&
						s.Category != ScriptCategory.NPC)
			.ToList();
	}

	/// <summary>
	/// Get all service scripts (inn, shop, NPC).
	/// </summary>
	public static List<EventScript> GetServiceScripts() {
		return GetAllScripts()
			.Where(s => s.Category == ScriptCategory.Inn ||
						s.Category == ScriptCategory.Shop ||
						s.Category == ScriptCategory.NPC)
			.ToList();
	}
}

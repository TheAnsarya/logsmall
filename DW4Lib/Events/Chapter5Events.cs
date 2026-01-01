using DW4Lib.Maps;

namespace DW4Lib.Events;

/// <summary>
/// Chapter 5 (Hero) event scripts.
/// Contains all scripted events, dialogs, and triggers for Chapter 5.
/// The Hero's village is destroyed by Psaro's forces, beginning the ultimate quest.
/// All previous chapter characters eventually join the Hero's party.
/// </summary>
public static class Chapter5Events {
	// ============================================================
	// Script IDs
	// ============================================================

	/// <summary>Chapter 5 script ID base.</summary>
	public const ushort ScriptBase = 0x0500;

	// --- Main Story Scripts ---

	/// <summary>Chapter 5 introduction - Hero's peaceful village.</summary>
	public const ushort IntroScript = ScriptBase + 0x00;

	/// <summary>Village under attack by monsters.</summary>
	public const ushort VillageAttack = ScriptBase + 0x01;

	/// <summary>Hero escapes through secret passage.</summary>
	public const ushort HeroEscape = ScriptBase + 0x02;

	/// <summary>Hero wakes alone in the wilderness.</summary>
	public const ushort HeroAwakens = ScriptBase + 0x03;

	/// <summary>Arrive at first town - Branca.</summary>
	public const ushort ArriveBranca = ScriptBase + 0x04;

	/// <summary>Learn about the Zenithian equipment.</summary>
	public const ushort ZenithianLegend = ScriptBase + 0x05;

	/// <summary>Meet Ragnar in Endor.</summary>
	public const ushort MeetRagnar = ScriptBase + 0x06;

	/// <summary>Ragnar joins the party.</summary>
	public const ushort RagnarJoins = ScriptBase + 0x07;

	/// <summary>Travel to Mintos.</summary>
	public const ushort TravelMintos = ScriptBase + 0x08;

	/// <summary>Meet Alena, Kiryl, and Borya.</summary>
	public const ushort MeetAlenaGroup = ScriptBase + 0x09;

	/// <summary>Alena's group joins.</summary>
	public const ushort AlenaGroupJoins = ScriptBase + 0x0A;

	/// <summary>Travel to the casino town.</summary>
	public const ushort TravelCasino = ScriptBase + 0x0B;

	/// <summary>Meet Torneko at the casino.</summary>
	public const ushort MeetTorneko = ScriptBase + 0x0C;

	/// <summary>Torneko joins.</summary>
	public const ushort TornekoJoins = ScriptBase + 0x0D;

	/// <summary>Meet Meena and Maya.</summary>
	public const ushort MeetSisters = ScriptBase + 0x0E;

	/// <summary>Meena and Maya join.</summary>
	public const ushort SistersJoin = ScriptBase + 0x0F;

	/// <summary>Find the Zenithian Sword.</summary>
	public const ushort FindZenithianSword = ScriptBase + 0x10;

	/// <summary>Find the Zenithian Armor.</summary>
	public const ushort FindZenithianArmor = ScriptBase + 0x11;

	/// <summary>Find the Zenithian Helm.</summary>
	public const ushort FindZenithianHelm = ScriptBase + 0x12;

	/// <summary>Find the Zenithian Shield.</summary>
	public const ushort FindZenithianShield = ScriptBase + 0x13;

	/// <summary>Access Zenithia.</summary>
	public const ushort AccessZenithia = ScriptBase + 0x14;

	/// <summary>Meet the Master Dragon.</summary>
	public const ushort MeetMasterDragon = ScriptBase + 0x15;

	/// <summary>Learn truth about Psaro.</summary>
	public const ushort PsaroTruth = ScriptBase + 0x16;

	/// <summary>Enter Psaro's castle.</summary>
	public const ushort EnterPsaroCastle = ScriptBase + 0x17;

	/// <summary>Final battle with Psaro.</summary>
	public const ushort PsaroBattle = ScriptBase + 0x18;

	/// <summary>Psaro defeated - ending sequence.</summary>
	public const ushort PsaroDefeated = ScriptBase + 0x19;

	/// <summary>Game ending.</summary>
	public const ushort GameEnding = ScriptBase + 0x1A;

	// --- Service Scripts ---

	/// <summary>Branca Inn.</summary>
	public const ushort BrancaInn = ScriptBase + 0x30;

	/// <summary>Branca Item Shop.</summary>
	public const ushort BrancaItemShop = ScriptBase + 0x31;

	/// <summary>Branca Weapon Shop.</summary>
	public const ushort BrancaWeaponShop = ScriptBase + 0x32;

	/// <summary>Branca Church.</summary>
	public const ushort BrancaChurch = ScriptBase + 0x33;

	/// <summary>Endor Inn.</summary>
	public const ushort EndorInn = ScriptBase + 0x34;

	/// <summary>Endor Item Shop.</summary>
	public const ushort EndorItemShop = ScriptBase + 0x35;

	/// <summary>Endor Weapon Shop.</summary>
	public const ushort EndorWeaponShop = ScriptBase + 0x36;

	/// <summary>Endor Church.</summary>
	public const ushort EndorChurch = ScriptBase + 0x37;

	/// <summary>Casino NPC.</summary>
	public const ushort CasinoNpc = ScriptBase + 0x38;

	/// <summary>Vault keeper.</summary>
	public const ushort VaultKeeper = ScriptBase + 0x39;

	// ============================================================
	// Flag IDs
	// ============================================================

	/// <summary>Village has been attacked.</summary>
	public const ushort FlagVillageAttacked = 0x0050;

	/// <summary>Hero escaped village.</summary>
	public const ushort FlagHeroEscaped = 0x0051;

	/// <summary>Arrived at Branca.</summary>
	public const ushort FlagArrivedBranca = 0x0052;

	/// <summary>Learned Zenithian legend.</summary>
	public const ushort FlagZenithianLegend = 0x0053;

	/// <summary>Met Ragnar.</summary>
	public const ushort FlagMetRagnar = 0x0054;

	/// <summary>Ragnar joined party.</summary>
	public const ushort FlagRagnarJoined = 0x0055;

	/// <summary>Met Alena's group.</summary>
	public const ushort FlagMetAlenaGroup = 0x0056;

	/// <summary>Alena's group joined.</summary>
	public const ushort FlagAlenaGroupJoined = 0x0057;

	/// <summary>Met Torneko.</summary>
	public const ushort FlagMetTorneko = 0x0058;

	/// <summary>Torneko joined.</summary>
	public const ushort FlagTornekoJoined = 0x0059;

	/// <summary>Met the sisters.</summary>
	public const ushort FlagMetSisters = 0x005A;

	/// <summary>Sisters joined.</summary>
	public const ushort FlagSistersJoined = 0x005B;

	/// <summary>Found Zenithian Sword.</summary>
	public const ushort FlagZenithianSword = 0x005C;

	/// <summary>Found Zenithian Armor.</summary>
	public const ushort FlagZenithianArmor = 0x005D;

	/// <summary>Found Zenithian Helm.</summary>
	public const ushort FlagZenithianHelm = 0x005E;

	/// <summary>Found Zenithian Shield.</summary>
	public const ushort FlagZenithianShield = 0x005F;

	/// <summary>All Zenithian equipment collected.</summary>
	public const ushort FlagAllZenithian = 0x0060;

	/// <summary>Accessed Zenithia.</summary>
	public const ushort FlagAccessedZenithia = 0x0061;

	/// <summary>Met Master Dragon.</summary>
	public const ushort FlagMetMasterDragon = 0x0062;

	/// <summary>Learned Psaro's truth.</summary>
	public const ushort FlagPsaroTruth = 0x0063;

	/// <summary>Entered Psaro's castle.</summary>
	public const ushort FlagEnteredPsaroCastle = 0x0064;

	/// <summary>Psaro defeated.</summary>
	public const ushort FlagPsaroDefeated = 0x0065;

	/// <summary>Game completed.</summary>
	public const ushort FlagGameComplete = 0x0066;

	// ============================================================
	// Music IDs
	// ============================================================

	/// <summary>Hero's peaceful village theme.</summary>
	public const byte MusicVillage = 0x50;

	/// <summary>Attack/emergency theme.</summary>
	public const byte MusicAttack = 0x51;

	/// <summary>Sad/tragedy theme.</summary>
	public const byte MusicTragedy = 0x52;

	/// <summary>Chapter 5 overworld theme.</summary>
	public const byte MusicOverworld = 0x53;

	/// <summary>Town theme.</summary>
	public const byte MusicTown = 0x54;

	/// <summary>Zenithia theme.</summary>
	public const byte MusicZenithia = 0x55;

	/// <summary>Psaro's castle theme.</summary>
	public const byte MusicPsaroCastle = 0x56;

	/// <summary>Final battle theme.</summary>
	public const byte MusicFinalBattle = 0x57;

	/// <summary>Victory/ending theme.</summary>
	public const byte MusicEnding = 0x58;

	// ============================================================
	// Item IDs
	// ============================================================

	/// <summary>Zenithian Sword.</summary>
	public const byte ItemZenithianSword = 0x80;

	/// <summary>Zenithian Armor.</summary>
	public const byte ItemZenithianArmor = 0x81;

	/// <summary>Zenithian Helm.</summary>
	public const byte ItemZenithianHelm = 0x82;

	/// <summary>Zenithian Shield.</summary>
	public const byte ItemZenithianShield = 0x83;

	/// <summary>Hero's Memento (from mother).</summary>
	public const byte ItemHeroMemento = 0x84;

	/// <summary>Baron's Horn (summons wagon).</summary>
	public const byte ItemBaronHorn = 0x85;

	/// <summary>Magic Key.</summary>
	public const byte ItemMagicKey = 0x86;

	/// <summary>Ultimate Key.</summary>
	public const byte ItemUltimateKey = 0x87;

	// ============================================================
	// Character IDs
	// ============================================================

	/// <summary>The Hero (player character).</summary>
	public const byte CharacterHero = 0x00;

	/// <summary>Ragnar McRyan.</summary>
	public const byte CharacterRagnar = 0x01;

	/// <summary>Alena.</summary>
	public const byte CharacterAlena = 0x02;

	/// <summary>Kiryl.</summary>
	public const byte CharacterKiryl = 0x03;

	/// <summary>Borya.</summary>
	public const byte CharacterBorya = 0x04;

	/// <summary>Meena.</summary>
	public const byte CharacterMeena = 0x05;

	/// <summary>Maya.</summary>
	public const byte CharacterMaya = 0x06;

	/// <summary>Torneko.</summary>
	public const byte CharacterTorneko = 0x07;

	// ============================================================
	// Monster IDs
	// ============================================================

	/// <summary>Psaro the Manslayer (human form).</summary>
	public const byte MonsterPsaroHuman = 0xF0;

	/// <summary>Psaro - first transformation.</summary>
	public const byte MonsterPsaroForm1 = 0xF1;

	/// <summary>Psaro - second transformation.</summary>
	public const byte MonsterPsaroForm2 = 0xF2;

	/// <summary>Psaro - third transformation.</summary>
	public const byte MonsterPsaroForm3 = 0xF3;

	/// <summary>Psaro - fourth transformation.</summary>
	public const byte MonsterPsaroForm4 = 0xF4;

	/// <summary>Psaro - fifth transformation.</summary>
	public const byte MonsterPsaroForm5 = 0xF5;

	/// <summary>Psaro - sixth transformation.</summary>
	public const byte MonsterPsaroForm6 = 0xF6;

	/// <summary>Psaro - final form.</summary>
	public const byte MonsterPsaroFinal = 0xF7;

	/// <summary>Master Dragon.</summary>
	public const byte MonsterMasterDragon = 0xF8;

	// ============================================================
	// Event Script Builders
	// ============================================================

	/// <summary>
	/// Get all Chapter 5 event scripts.
	/// </summary>
	public static EventScript[] GetAllScripts() => [
		BuildIntroScript(),
		BuildVillageAttackScript(),
		BuildHeroEscapeScript(),
		BuildHeroAwakensScript(),
		BuildArriveBrancaScript(),
		BuildZenithianLegendScript(),
		BuildMeetRagnarScript(),
		BuildRagnarJoinsScript(),
		BuildMeetAlenaGroupScript(),
		BuildAlenaGroupJoinsScript(),
		BuildMeetTornekoScript(),
		BuildTornekoJoinsScript(),
		BuildMeetSistersScript(),
		BuildSistersJoinScript(),
		BuildFindZenithianSwordScript(),
		BuildFindZenithianArmorScript(),
		BuildFindZenithianHelmScript(),
		BuildFindZenithianShieldScript(),
		BuildAccessZenithiaScript(),
		BuildMeetMasterDragonScript(),
		BuildPsaroTruthScript(),
		BuildEnterPsaroCastleScript(),
		BuildPsaroBattleScript(),
		BuildPsaroDefeatedScript(),
		BuildGameEndingScript(),
		// Service scripts
		BuildBrancaInnScript(),
		BuildBrancaItemShopScript(),
		BuildBrancaWeaponShopScript(),
		BuildBrancaChurchScript(),
		BuildEndorInnScript(),
		BuildEndorItemShopScript(),
		BuildEndorWeaponShopScript(),
		BuildEndorChurchScript(),
		BuildCasinoNpcScript(),
		BuildVaultKeeperScript()
	];

	// ============================================================
	// Story Scripts
	// ============================================================

	/// <summary>
	/// Chapter 5 introduction - Hero's peaceful village before the attack.
	/// </summary>
	public static EventScript BuildIntroScript() {
		return new EventScriptBuilder(IntroScript)
			.WithName("Chapter 5 Intro")
			.WithCategory(ScriptCategory.Cutscene)
			.ForChapter(4)
			.FadeOut()
			.PlayMusic(MusicVillage)
			.ShowDialog(0x0800) // "Chapter 5: The Chosen"
			.ShowDialog(0x0801) // Narrator: In a remote village...
			.ShowDialog(0x0802) // A child born with a special destiny...
			.ShowDialog(0x0803) // Protected by the villagers...
			.ShowDialog(0x0804) // But evil forces are searching...
			.FadeIn()
			.ShowDialog(0x0805) // Mother: "It's your birthday today!"
			.ShowDialog(0x0806) // Mother: "Your father would be proud..."
			.ShowDialog(0x0807) // Mother: "Here, take this memento."
			.GiveItem(ItemHeroMemento)
			.ShowDialog(0x0808) // "Obtained Hero's Memento!"
			.End()
			.Build();
	}

	/// <summary>
	/// Village under attack by monsters.
	/// </summary>
	public static EventScript BuildVillageAttackScript() {
		return new EventScriptBuilder(VillageAttack)
			.WithName("Village Attack")
			.WithCategory(ScriptCategory.Cutscene)
			.ForChapter(4)
			.CheckFlag(FlagVillageAttacked, 0x0020)
			// Attack sequence
			.PlayMusic(MusicAttack)
			.ShowDialog(0x0810) // "Villager: Monsters! Monsters are attacking!"
			.ShowDialog(0x0811) // Screams and chaos
			.ShowDialog(0x0812) // Mother: "Quick! You must escape!"
			.ShowDialog(0x0813) // Mother: "Through the secret passage..."
			.ShowDialog(0x0814) // Mother: "Find the Zenithian... they will help you..."
			.SetFlag(FlagVillageAttacked)
			.End()
			// Already happened
			.ShowDialog(0x0815) // The village is in ruins...
			.End()
			.Build();
	}

	/// <summary>
	/// Hero escapes through secret passage.
	/// </summary>
	public static EventScript BuildHeroEscapeScript() {
		return new EventScriptBuilder(HeroEscape)
			.WithName("Hero Escape")
			.WithCategory(ScriptCategory.Cutscene)
			.ForChapter(4)
			.CheckFlag(FlagHeroEscaped, 0x0020)
			// Escape sequence
			.PlayMusic(MusicTragedy)
			.ShowDialog(0x0820) // Hero runs through the passage
			.ShowDialog(0x0821) // Collapse behind
			.ShowDialog(0x0822) // Cannot return
			.FadeOut()
			.ShowDialog(0x0823) // Everything goes dark...
			.SetFlag(FlagHeroEscaped)
			.End()
			// Already escaped
			.ShowDialog(0x0824) // The passage is sealed.
			.End()
			.Build();
	}

	/// <summary>
	/// Hero wakes alone in the wilderness.
	/// </summary>
	public static EventScript BuildHeroAwakensScript() {
		return new EventScriptBuilder(HeroAwakens)
			.WithName("Hero Awakens")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(4)
			.PlayMusic(MusicOverworld)
			.FadeIn()
			.ShowDialog(0x0830) // Hero wakes up
			.ShowDialog(0x0831) // Alone in an unfamiliar place
			.ShowDialog(0x0832) // Must find help
			.ShowDialog(0x0833) // Remember mother's words...
			.End()
			.Build();
	}

	/// <summary>
	/// Arrive at Branca - first town.
	/// </summary>
	public static EventScript BuildArriveBrancaScript() {
		return new EventScriptBuilder(ArriveBranca)
			.WithName("Arrive Branca")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(4)
			.CheckFlag(FlagArrivedBranca, 0x0020)
			// First time
			.PlayMusic(MusicTown)
			.ShowDialog(0x0840) // Welcome to Branca
			.ShowDialog(0x0841) // Townspeople are friendly
			.ShowDialog(0x0842) // Ask about Zenithian equipment
			.SetFlag(FlagArrivedBranca)
			.End()
			// Already visited
			.PlayMusic(MusicTown)
			.End()
			.Build();
	}

	/// <summary>
	/// Learn about the Zenithian legend.
	/// </summary>
	public static EventScript BuildZenithianLegendScript() {
		return new EventScriptBuilder(ZenithianLegend)
			.WithName("Zenithian Legend")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(4)
			.CheckFlag(FlagZenithianLegend, 0x0020)
			// First time
			.ShowDialog(0x0850) // Elder: "You seek the Zenithian equipment?"
			.ShowDialog(0x0851) // "Four legendary items..."
			.ShowDialog(0x0852) // "The Sword... hidden in the depths..."
			.ShowDialog(0x0853) // "The Armor... guarded by a great beast..."
			.ShowDialog(0x0854) // "The Helm... in Zenithia itself..."
			.ShowDialog(0x0855) // "The Shield... in a castle of darkness..."
			.ShowDialog(0x0856) // "Only the Chosen One can wield them..."
			.SetFlag(FlagZenithianLegend)
			.End()
			// Already learned
			.ShowDialog(0x0857) // "May the Goddess guide you..."
			.End()
			.Build();
	}

	/// <summary>
	/// Meet Ragnar in Endor.
	/// </summary>
	public static EventScript BuildMeetRagnarScript() {
		return new EventScriptBuilder(MeetRagnar)
			.WithName("Meet Ragnar")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(4)
			.CheckFlag(FlagMetRagnar, 0x0020)
			// First time
			.ShowDialog(0x0860) // "Ragnar: Halt! You there!"
			.ShowDialog(0x0861) // "You... you're the one from the prophecy!"
			.ShowDialog(0x0862) // "I am Ragnar, knight of Burland."
			.ShowDialog(0x0863) // "I have been searching for you."
			.SetFlag(FlagMetRagnar)
			.End()
			// Already met
			.ShowDialog(0x0864) // "Let us continue our quest!"
			.End()
			.Build();
	}

	/// <summary>
	/// Ragnar joins the party.
	/// </summary>
	public static EventScript BuildRagnarJoinsScript() {
		return new EventScriptBuilder(RagnarJoins)
			.WithName("Ragnar Joins")
			.WithCategory(ScriptCategory.Cutscene)
			.ForChapter(4)
			.CheckFlag(FlagRagnarJoined, 0x0020)
			// Join sequence
			.ShowDialog(0x0870) // "Ragnar: Allow me to join you!"
			.ShowDialog(0x0871) // "My sword is yours to command."
			.AddPartyMember(CharacterRagnar)
			.ShowDialog(0x0872) // "Ragnar joined the party!"
			.SetFlag(FlagRagnarJoined)
			.End()
			// Already joined
			.ShowDialog(0x0873) // "Ragnar is already in your party."
			.End()
			.Build();
	}

	/// <summary>
	/// Meet Alena's group in Mintos.
	/// </summary>
	public static EventScript BuildMeetAlenaGroupScript() {
		return new EventScriptBuilder(MeetAlenaGroup)
			.WithName("Meet Alena Group")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(4)
			.CheckFlag(FlagMetAlenaGroup, 0x0020)
			// First time
			.ShowDialog(0x0880) // "Alena: Who goes there?!"
			.ShowDialog(0x0881) // Kiryl: "Princess, wait..."
			.ShowDialog(0x0882) // Borya: "This one has a special aura..."
			.ShowDialog(0x0883) // "Alena: You're the Chosen One!"
			.SetFlag(FlagMetAlenaGroup)
			.End()
			// Already met
			.ShowDialog(0x0884) // "Ready for adventure!"
			.End()
			.Build();
	}

	/// <summary>
	/// Alena's group joins the party.
	/// </summary>
	public static EventScript BuildAlenaGroupJoinsScript() {
		return new EventScriptBuilder(AlenaGroupJoins)
			.WithName("Alena Group Joins")
			.WithCategory(ScriptCategory.Cutscene)
			.ForChapter(4)
			.CheckFlag(FlagAlenaGroupJoined, 0x0020)
			// Join sequence
			.ShowDialog(0x0890) // "Alena: We will fight alongside you!"
			.ShowDialog(0x0891) // "Kiryl: And I shall heal your wounds."
			.ShowDialog(0x0892) // "Borya: My magic is at your service."
			.AddPartyMember(CharacterAlena)
			.AddPartyMember(CharacterKiryl)
			.AddPartyMember(CharacterBorya)
			.ShowDialog(0x0893) // "Alena, Kiryl, and Borya joined!"
			.SetFlag(FlagAlenaGroupJoined)
			.End()
			// Already joined
			.ShowDialog(0x0894) // "They're already with you."
			.End()
			.Build();
	}

	/// <summary>
	/// Meet Torneko at the casino.
	/// </summary>
	public static EventScript BuildMeetTornekoScript() {
		return new EventScriptBuilder(MeetTorneko)
			.WithName("Meet Torneko")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(4)
			.CheckFlag(FlagMetTorneko, 0x0020)
			// First time
			.ShowDialog(0x08A0) // "Torneko: Well, well! A customer?"
			.ShowDialog(0x08A1) // "Wait... you're the Chosen One!"
			.ShowDialog(0x08A2) // "I've been waiting for this day!"
			.SetFlag(FlagMetTorneko)
			.End()
			// Already met
			.ShowDialog(0x08A3) // "Got any treasures to sell?"
			.End()
			.Build();
	}

	/// <summary>
	/// Torneko joins the party.
	/// </summary>
	public static EventScript BuildTornekoJoinsScript() {
		return new EventScriptBuilder(TornekoJoins)
			.WithName("Torneko Joins")
			.WithCategory(ScriptCategory.Cutscene)
			.ForChapter(4)
			.CheckFlag(FlagTornekoJoined, 0x0020)
			// Join sequence
			.ShowDialog(0x08B0) // "Torneko: I shall accompany you!"
			.ShowDialog(0x08B1) // "My merchant skills will prove useful!"
			.AddPartyMember(CharacterTorneko)
			.ShowDialog(0x08B2) // "Torneko joined the party!"
			.SetFlag(FlagTornekoJoined)
			.End()
			// Already joined
			.ShowDialog(0x08B3) // "Torneko is already with you."
			.End()
			.Build();
	}

	/// <summary>
	/// Meet Meena and Maya.
	/// </summary>
	public static EventScript BuildMeetSistersScript() {
		return new EventScriptBuilder(MeetSisters)
			.WithName("Meet Sisters")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(4)
			.CheckFlag(FlagMetSisters, 0x0020)
			// First time
			.ShowDialog(0x08C0) // "Maya: Another dancer? No wait..."
			.ShowDialog(0x08C1) // "Meena: I sense a great destiny..."
			.ShowDialog(0x08C2) // "You are the one who can help us!"
			.SetFlag(FlagMetSisters)
			.End()
			// Already met
			.ShowDialog(0x08C3) // "The sisters nod at you."
			.End()
			.Build();
	}

	/// <summary>
	/// Meena and Maya join the party.
	/// </summary>
	public static EventScript BuildSistersJoinScript() {
		return new EventScriptBuilder(SistersJoin)
			.WithName("Sisters Join")
			.WithCategory(ScriptCategory.Cutscene)
			.ForChapter(4)
			.CheckFlag(FlagSistersJoined, 0x0020)
			// Join sequence
			.ShowDialog(0x08D0) // "Maya: We'll join your fight!"
			.ShowDialog(0x08D1) // "Meena: Our father's vengeance awaits."
			.AddPartyMember(CharacterMeena)
			.AddPartyMember(CharacterMaya)
			.ShowDialog(0x08D2) // "Meena and Maya joined the party!"
			.SetFlag(FlagSistersJoined)
			.End()
			// Already joined
			.ShowDialog(0x08D3) // "The sisters are ready."
			.End()
			.Build();
	}

	/// <summary>
	/// Find the Zenithian Sword.
	/// </summary>
	public static EventScript BuildFindZenithianSwordScript() {
		return new EventScriptBuilder(FindZenithianSword)
			.WithName("Find Zenithian Sword")
			.WithCategory(ScriptCategory.Item)
			.ForChapter(4)
			.CheckFlag(FlagZenithianSword, 0x0020)
			// First time
			.ShowDialog(0x08E0) // "A brilliant light emanates from the chest!"
			.GiveItem(ItemZenithianSword)
			.ShowDialog(0x08E1) // "Obtained Zenithian Sword!"
			.ShowDialog(0x08E2) // "The legendary blade responds to your touch!"
			.SetFlag(FlagZenithianSword)
			.End()
			// Already obtained
			.ShowDialog(0x08E3) // "The chest is empty."
			.End()
			.Build();
	}

	/// <summary>
	/// Find the Zenithian Armor.
	/// </summary>
	public static EventScript BuildFindZenithianArmorScript() {
		return new EventScriptBuilder(FindZenithianArmor)
			.WithName("Find Zenithian Armor")
			.WithCategory(ScriptCategory.Item)
			.ForChapter(4)
			.CheckFlag(FlagZenithianArmor, 0x0020)
			// First time
			.ShowDialog(0x08F0) // "The armor glows with holy light!"
			.GiveItem(ItemZenithianArmor)
			.ShowDialog(0x08F1) // "Obtained Zenithian Armor!"
			.ShowDialog(0x08F2) // "Divine protection wraps around you!"
			.SetFlag(FlagZenithianArmor)
			.End()
			// Already obtained
			.ShowDialog(0x08F3) // "The pedestal is empty."
			.End()
			.Build();
	}

	/// <summary>
	/// Find the Zenithian Helm.
	/// </summary>
	public static EventScript BuildFindZenithianHelmScript() {
		return new EventScriptBuilder(FindZenithianHelm)
			.WithName("Find Zenithian Helm")
			.WithCategory(ScriptCategory.Item)
			.ForChapter(4)
			.CheckFlag(FlagZenithianHelm, 0x0020)
			// First time
			.ShowDialog(0x0900) // "The helm radiates divine energy!"
			.GiveItem(ItemZenithianHelm)
			.ShowDialog(0x0901) // "Obtained Zenithian Helm!"
			.ShowDialog(0x0902) // "Wisdom of the ancients fills your mind!"
			.SetFlag(FlagZenithianHelm)
			.End()
			// Already obtained
			.ShowDialog(0x0903) // "The altar is empty."
			.End()
			.Build();
	}

	/// <summary>
	/// Find the Zenithian Shield.
	/// </summary>
	public static EventScript BuildFindZenithianShieldScript() {
		return new EventScriptBuilder(FindZenithianShield)
			.WithName("Find Zenithian Shield")
			.WithCategory(ScriptCategory.Item)
			.ForChapter(4)
			.CheckFlag(FlagZenithianShield, 0x0020)
			// First time
			.ShowDialog(0x0910) // "The shield shimmers with starlight!"
			.GiveItem(ItemZenithianShield)
			.ShowDialog(0x0911) // "Obtained Zenithian Shield!"
			.ShowDialog(0x0912) // "The last piece of the legend..."
			// Check if all pieces collected
			.CheckFlag(FlagZenithianSword, 0x0008) // If sword found
			.CheckFlag(FlagZenithianArmor, 0x0008) // If armor found
			.CheckFlag(FlagZenithianHelm, 0x0008) // If helm found
			.SetFlag(FlagZenithianShield)
			.SetFlag(FlagAllZenithian)
			.ShowDialog(0x0913) // "All Zenithian equipment collected!"
			.End()
			// Already obtained
			.ShowDialog(0x0914) // "The shrine is empty."
			.End()
			.Build();
	}

	/// <summary>
	/// Access Zenithia - the floating castle.
	/// </summary>
	public static EventScript BuildAccessZenithiaScript() {
		return new EventScriptBuilder(AccessZenithia)
			.WithName("Access Zenithia")
			.WithCategory(ScriptCategory.Cutscene)
			.ForChapter(4)
			.CheckFlag(FlagAccessedZenithia, 0x0020)
			// First time - must have all equipment
			.CheckFlag(FlagAllZenithian, 0x0010)
			// Missing equipment
			.ShowDialog(0x0920) // "The path to Zenithia remains sealed..."
			.ShowDialog(0x0921) // "Gather all Zenithian equipment..."
			.End()
			// Has all equipment
			.FadeOut()
			.PlayMusic(MusicZenithia)
			.ShowDialog(0x0922) // "The equipment resonates..."
			.ShowDialog(0x0923) // "A bridge of light appears!"
			.FadeIn()
			.ShowDialog(0x0924) // "Welcome to Zenithia!"
			.SetFlag(FlagAccessedZenithia)
			.End()
			// Already accessed
			.PlayMusic(MusicZenithia)
			.End()
			.Build();
	}

	/// <summary>
	/// Meet the Master Dragon in Zenithia.
	/// </summary>
	public static EventScript BuildMeetMasterDragonScript() {
		return new EventScriptBuilder(MeetMasterDragon)
			.WithName("Meet Master Dragon")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(4)
			.CheckFlag(FlagMetMasterDragon, 0x0020)
			// First time
			.ShowDialog(0x0930) // "Master Dragon: Welcome, Chosen One..."
			.ShowDialog(0x0931) // "I have awaited your arrival."
			.ShowDialog(0x0932) // "The time has come to face Psaro..."
			.ShowDialog(0x0933) // "But first, hear the truth..."
			.SetFlag(FlagMetMasterDragon)
			.End()
			// Already met
			.ShowDialog(0x0934) // "The fate of the world rests with you."
			.End()
			.Build();
	}

	/// <summary>
	/// Learn the truth about Psaro.
	/// </summary>
	public static EventScript BuildPsaroTruthScript() {
		return new EventScriptBuilder(PsaroTruth)
			.WithName("Psaro Truth")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(4)
			.CheckFlag(FlagPsaroTruth, 0x0020)
			// First time
			.ShowDialog(0x0940) // "Master Dragon: Psaro was not always evil..."
			.ShowDialog(0x0941) // "His beloved Rose was slain by humans..."
			.ShowDialog(0x0942) // "In his grief, he embraced darkness..."
			.ShowDialog(0x0943) // "Now he seeks to destroy all humanity..."
			.ShowDialog(0x0944) // "You must stop him, or all is lost."
			.SetFlag(FlagPsaroTruth)
			.End()
			// Already learned
			.ShowDialog(0x0945) // "Save the world from Psaro's wrath."
			.End()
			.Build();
	}

	/// <summary>
	/// Enter Psaro's castle.
	/// </summary>
	public static EventScript BuildEnterPsaroCastleScript() {
		return new EventScriptBuilder(EnterPsaroCastle)
			.WithName("Enter Psaro Castle")
			.WithCategory(ScriptCategory.Cutscene)
			.ForChapter(4)
			.CheckFlag(FlagEnteredPsaroCastle, 0x0020)
			// First time
			.FadeOut()
			.PlayMusic(MusicPsaroCastle)
			.FadeIn()
			.ShowDialog(0x0950) // "The dark castle looms before you..."
			.ShowDialog(0x0951) // "Psaro awaits within..."
			.ShowDialog(0x0952) // "This is the final battle!"
			.SetFlag(FlagEnteredPsaroCastle)
			.End()
			// Already entered
			.PlayMusic(MusicPsaroCastle)
			.End()
			.Build();
	}

	/// <summary>
	/// Final battle with Psaro.
	/// </summary>
	public static EventScript BuildPsaroBattleScript() {
		return new EventScriptBuilder(PsaroBattle)
			.WithName("Psaro Battle")
			.WithCategory(ScriptCategory.Battle)
			.ForChapter(4)
			.CheckFlag(FlagPsaroDefeated, 0x0020)
			// Battle sequence
			.ShowDialog(0x0960) // "Psaro: So you've come, Chosen One..."
			.ShowDialog(0x0961) // "Psaro: I will destroy you and all humanity!"
			.PlayMusic(MusicFinalBattle)
			.StartBattle(MonsterPsaroHuman)
			// Multi-form battle continues automatically
			.End()
			// Already defeated
			.ShowDialog(0x0962) // "Psaro has been vanquished."
			.End()
			.Build();
	}

	/// <summary>
	/// Psaro defeated - victory sequence.
	/// </summary>
	public static EventScript BuildPsaroDefeatedScript() {
		return new EventScriptBuilder(PsaroDefeated)
			.WithName("Psaro Defeated")
			.WithCategory(ScriptCategory.Cutscene)
			.ForChapter(4)
			.FadeOut()
			.ShowDialog(0x0970) // "Psaro falls..."
			.ShowDialog(0x0971) // "The darkness begins to fade..."
			.ShowDialog(0x0972) // "Light returns to the world!"
			.SetFlag(FlagPsaroDefeated)
			.FadeIn()
			.End()
			.Build();
	}

	/// <summary>
	/// Game ending sequence.
	/// </summary>
	public static EventScript BuildGameEndingScript() {
		return new EventScriptBuilder(GameEnding)
			.WithName("Game Ending")
			.WithCategory(ScriptCategory.Cutscene)
			.ForChapter(4)
			.FadeOut()
			.PlayMusic(MusicEnding)
			.ShowDialog(0x0980) // "Peace has returned to the world..."
			.ShowDialog(0x0981) // "The Chosen One's legend is complete."
			.ShowDialog(0x0982) // Each party member's epilogue...
			.ShowDialog(0x0983) // "Ragnar returns to Burland..."
			.ShowDialog(0x0984) // "Alena takes the throne of Santeem..."
			.ShowDialog(0x0985) // "Torneko opens his dream shop..."
			.ShowDialog(0x0986) // "Meena and Maya find peace..."
			.ShowDialog(0x0987) // "And the Hero..."
			.ShowDialog(0x0988) // "THE END"
			.SetFlag(FlagGameComplete)
			.FadeIn()
			.End()
			.Build();
	}

	// ============================================================
	// Service Scripts
	// ============================================================

	/// <summary>
	/// Branca Inn service.
	/// </summary>
	public static EventScript BuildBrancaInnScript() {
		return new EventScriptBuilder(BrancaInn)
			.WithName("Branca Inn")
			.WithCategory(ScriptCategory.Inn)
			.ForChapter(4)
			.ShowDialog(0x0A00) // "Innkeeper: Welcome! Rest for 10 gold?"
			.OpenInn(0x50, 10)
			.Return()
			.Build();
	}

	/// <summary>
	/// Branca Item shop service.
	/// </summary>
	public static EventScript BuildBrancaItemShopScript() {
		return new EventScriptBuilder(BrancaItemShop)
			.WithName("Branca Item Shop")
			.WithCategory(ScriptCategory.Shop)
			.ForChapter(4)
			.ShowDialog(0x0A10) // "Shopkeeper: What can I get you?"
			.OpenShop(0x50)
			.Return()
			.Build();
	}

	/// <summary>
	/// Branca Weapon shop service.
	/// </summary>
	public static EventScript BuildBrancaWeaponShopScript() {
		return new EventScriptBuilder(BrancaWeaponShop)
			.WithName("Branca Weapon Shop")
			.WithCategory(ScriptCategory.Shop)
			.ForChapter(4)
			.ShowDialog(0x0A20) // "Armorer: Need equipment?"
			.OpenShop(0x51)
			.Return()
			.Build();
	}

	/// <summary>
	/// Branca Church service.
	/// </summary>
	public static EventScript BuildBrancaChurchScript() {
		return new EventScriptBuilder(BrancaChurch)
			.WithName("Branca Church")
			.WithCategory(ScriptCategory.NPC)
			.ForChapter(4)
			.ShowDialog(0x0A30) // "Priest: Blessings upon you."
			.OpenChurch()
			.Return()
			.Build();
	}

	/// <summary>
	/// Endor Inn service.
	/// </summary>
	public static EventScript BuildEndorInnScript() {
		return new EventScriptBuilder(EndorInn)
			.WithName("Endor Inn")
			.WithCategory(ScriptCategory.Inn)
			.ForChapter(4)
			.ShowDialog(0x0A40) // "Innkeeper: Rest for 30 gold?"
			.OpenInn(0x51, 30)
			.Return()
			.Build();
	}

	/// <summary>
	/// Endor Item shop service.
	/// </summary>
	public static EventScript BuildEndorItemShopScript() {
		return new EventScriptBuilder(EndorItemShop)
			.WithName("Endor Item Shop")
			.WithCategory(ScriptCategory.Shop)
			.ForChapter(4)
			.ShowDialog(0x0A50) // "Shopkeeper: Welcome to Endor!"
			.OpenShop(0x52)
			.Return()
			.Build();
	}

	/// <summary>
	/// Endor Weapon shop service.
	/// </summary>
	public static EventScript BuildEndorWeaponShopScript() {
		return new EventScriptBuilder(EndorWeaponShop)
			.WithName("Endor Weapon Shop")
			.WithCategory(ScriptCategory.Shop)
			.ForChapter(4)
			.ShowDialog(0x0A60) // "Armorer: Finest weapons here!"
			.OpenShop(0x53)
			.Return()
			.Build();
	}

	/// <summary>
	/// Endor Church service.
	/// </summary>
	public static EventScript BuildEndorChurchScript() {
		return new EventScriptBuilder(EndorChurch)
			.WithName("Endor Church")
			.WithCategory(ScriptCategory.NPC)
			.ForChapter(4)
			.ShowDialog(0x0A70) // "Priest: May light guide your path."
			.OpenChurch()
			.Return()
			.Build();
	}

	/// <summary>
	/// Casino NPC.
	/// </summary>
	public static EventScript BuildCasinoNpcScript() {
		return new EventScriptBuilder(CasinoNpc)
			.WithName("Casino NPC")
			.WithCategory(ScriptCategory.NPC)
			.ForChapter(4)
			.ShowDialog(0x0A80) // "Welcome to the Casino!"
			.ShowDialog(0x0A81) // "Try your luck at the slots!"
			.Return()
			.Build();
	}

	/// <summary>
	/// Vault keeper.
	/// </summary>
	public static EventScript BuildVaultKeeperScript() {
		return new EventScriptBuilder(VaultKeeper)
			.WithName("Vault Keeper")
			.WithCategory(ScriptCategory.NPC)
			.ForChapter(4)
			.ShowDialog(0x0A90) // "Vault Keeper: Store your items safely here."
			.Return()
			.Build();
	}

	/// <summary>
	/// Get all story scripts (non-service).
	/// </summary>
	public static List<EventScript> GetStoryScripts() {
		return [.. GetAllScripts()
			.Where(s => s.Category != ScriptCategory.Inn &&
						s.Category != ScriptCategory.Shop &&
						s.Category != ScriptCategory.NPC)];
	}

	/// <summary>
	/// Get all service scripts (inn, shop, NPC).
	/// </summary>
	public static List<EventScript> GetServiceScripts() {
		return [.. GetAllScripts()
			.Where(s => s.Category == ScriptCategory.Inn ||
						s.Category == ScriptCategory.Shop ||
						s.Category == ScriptCategory.NPC)];
	}
}

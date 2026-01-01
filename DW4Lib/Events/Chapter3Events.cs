namespace DW4Lib.Events;

/// <summary>
/// Chapter 3 (Taloon/Torneko) event scripts.
/// Contains all scripted events, dialogs, and triggers for Chapter 3.
/// Taloon's chapter features unique merchant/shopkeeping mechanics.
/// </summary>
public static class Chapter3Events {
	// ============================================================
	// Script IDs
	// ============================================================

	/// <summary>Chapter 3 script ID base.</summary>
	public const ushort ScriptBase = 0x0300;

	/// <summary>Chapter intro script.</summary>
	public const ushort IntroScript = ScriptBase + 0x00;

	/// <summary>Morning at home with Neta.</summary>
	public const ushort MorningHome = ScriptBase + 0x01;

	/// <summary>First day at the weapon shop.</summary>
	public const ushort FirstDayShop = ScriptBase + 0x02;

	/// <summary>Shop tutorial - buying from customers.</summary>
	public const ushort ShopTutorialBuy = ScriptBase + 0x03;

	/// <summary>Shop tutorial - selling to customers.</summary>
	public const ushort ShopTutorialSell = ScriptBase + 0x04;

	/// <summary>Earn enough for iron safe.</summary>
	public const ushort EarnIronSafe = ScriptBase + 0x05;

	/// <summary>Old man tells about the cave.</summary>
	public const ushort OldManCave = ScriptBase + 0x06;

	/// <summary>First cave exploration.</summary>
	public const ushort CaveFirst = ScriptBase + 0x07;

	/// <summary>Find the Steel Broadsword.</summary>
	public const ushort FindSteelSword = ScriptBase + 0x08;

	/// <summary>Return sword to shop.</summary>
	public const ushort ReturnSword = ScriptBase + 0x09;

	/// <summary>Fox village discovery.</summary>
	public const ushort FoxVillage = ScriptBase + 0x0A;

	/// <summary>Prince Reed's request.</summary>
	public const ushort PrinceReed = ScriptBase + 0x0B;

	/// <summary>Cave of Silver Statuette.</summary>
	public const ushort SilverStatuetteCave = ScriptBase + 0x0C;

	/// <summary>Deliver Silver Statuette.</summary>
	public const ushort DeliverStatuette = ScriptBase + 0x0D;

	/// <summary>Earn enough to open own shop.</summary>
	public const ushort OpenOwnShop = ScriptBase + 0x0E;

	/// <summary>Ship passage to Endor.</summary>
	public const ushort ShipPassage = ScriptBase + 0x0F;

	/// <summary>Arrive at Endor.</summary>
	public const ushort ArriveEndor = ScriptBase + 0x10;

	/// <summary>Meet with arms dealer.</summary>
	public const ushort MeetArmsDealer = ScriptBase + 0x11;

	/// <summary>Tunnel construction started.</summary>
	public const ushort TunnelStart = ScriptBase + 0x12;

	/// <summary>Tunnel construction complete.</summary>
	public const ushort TunnelComplete = ScriptBase + 0x13;

	/// <summary>Chapter complete.</summary>
	public const ushort ChapterComplete = ScriptBase + 0x14;

	// ============================================================
	// Service Script IDs
	// ============================================================

	/// <summary>Lakanaba weapon shop (work here).</summary>
	public const ushort LakanabaWeaponShop = ScriptBase + 0x20;

	/// <summary>Lakanaba item shop.</summary>
	public const ushort LakanabaItemShop = ScriptBase + 0x21;

	/// <summary>Lakanaba inn.</summary>
	public const ushort LakanabaInn = ScriptBase + 0x22;

	/// <summary>Lakanaba church.</summary>
	public const ushort LakanabaChurch = ScriptBase + 0x23;

	/// <summary>Foxville shop (foxes).</summary>
	public const ushort FoxvilleShop = ScriptBase + 0x24;

	/// <summary>Bonmalmo shop.</summary>
	public const ushort BonmalmoShop = ScriptBase + 0x25;

	/// <summary>Bonmalmo inn.</summary>
	public const ushort BonmalmoInn = ScriptBase + 0x26;

	/// <summary>Endor weapon shop.</summary>
	public const ushort EndorWeaponShop = ScriptBase + 0x27;

	/// <summary>Endor inn.</summary>
	public const ushort EndorInn = ScriptBase + 0x28;

	// ============================================================
	// Flag IDs
	// ============================================================

	/// <summary>Left home for first day.</summary>
	public const ushort FlagLeftHome = 0x0200;

	/// <summary>Completed shop tutorial.</summary>
	public const ushort FlagShopTutorial = 0x0201;

	/// <summary>Earned enough for iron safe.</summary>
	public const ushort FlagIronSafe = 0x0202;

	/// <summary>Heard about cave from old man.</summary>
	public const ushort FlagOldManCave = 0x0203;

	/// <summary>Found Steel Broadsword.</summary>
	public const ushort FlagFoundSword = 0x0204;

	/// <summary>Discovered Fox village.</summary>
	public const ushort FlagFoxVillage = 0x0205;

	/// <summary>Talked to Prince Reed.</summary>
	public const ushort FlagPrinceReed = 0x0206;

	/// <summary>Delivered Silver Statuette.</summary>
	public const ushort FlagStatuette = 0x0207;

	/// <summary>Can open own shop.</summary>
	public const ushort FlagOwnShop = 0x0208;

	/// <summary>Got ship passage.</summary>
	public const ushort FlagShipPassage = 0x0209;

	/// <summary>Met arms dealer in Endor.</summary>
	public const ushort FlagArmsDealer = 0x020A;

	/// <summary>Tunnel construction started.</summary>
	public const ushort FlagTunnelStart = 0x020B;

	/// <summary>Tunnel is complete.</summary>
	public const ushort FlagTunnelDone = 0x020C;

	// ============================================================
	// Character IDs
	// ============================================================

	/// <summary>Taloon (main character).</summary>
	public const byte CharTaloon = 0x03;

	/// <summary>Neta (Taloon's wife).</summary>
	public const byte CharNeta = 0x43;

	/// <summary>Lakanaba shop owner.</summary>
	public const byte CharShopOwner = 0x44;

	/// <summary>Old man (cave info).</summary>
	public const byte CharOldMan = 0x45;

	/// <summary>Prince Reed.</summary>
	public const byte CharPrinceReed = 0x46;

	/// <summary>Arms dealer.</summary>
	public const byte CharArmsDealer = 0x47;

	// ============================================================
	// Item IDs
	// ============================================================

	/// <summary>Iron Safe (storage upgrade).</summary>
	public const byte ItemIronSafe = 0x60;

	/// <summary>Steel Broadsword (special find).</summary>
	public const byte ItemSteelSword = 0x15;

	/// <summary>Silver Statuette (quest item).</summary>
	public const byte ItemSilverStatuette = 0x61;

	/// <summary>Ship Ticket.</summary>
	public const byte ItemShipTicket = 0x62;

	// ============================================================
	// Music IDs
	// ============================================================

	/// <summary>Chapter 3 theme (merchant song).</summary>
	public const byte MusicChapter3 = 0x20;

	/// <summary>Shop music.</summary>
	public const byte MusicShop = 0x21;

	/// <summary>Dungeon music.</summary>
	public const byte MusicDungeon = 0x06;

	/// <summary>Overworld music.</summary>
	public const byte MusicOverworld = 0x04;

	/// <summary>Victory fanfare.</summary>
	public const byte MusicVictory = 0x10;

	// ============================================================
	// Script Builders
	// ============================================================

	/// <summary>
	/// Chapter 3 intro - Taloon at home with Neta.
	/// </summary>
	public static EventScript BuildIntroScript() {
		return new EventScriptBuilder(IntroScript)
			.WithName("Chapter 3 Intro")
			.WithCategory(ScriptCategory.Cutscene)
			.ForChapter(2)
			.FadeOut()
			.PlayMusic(MusicChapter3)
			.FadeIn()
			.ShowDialog(0x0300) // "In the town of Lakanaba..."
			.ShowDialog(0x0301) // "...lived a humble arms merchant named Taloon."
			.ShowDialog(0x0302) // "His dream: to become the world's greatest merchant."
			.Wait(30)
			.End()
			.Build();
	}

	/// <summary>
	/// Morning at home with Neta, before first day at shop.
	/// </summary>
	public static EventScript BuildMorningHomeScript() {
		return new EventScriptBuilder(MorningHome)
			.WithName("Morning Home")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(2)
			.CheckFlag(FlagLeftHome, 0x0010)
			// First morning
			.ShowDialog(0x0303) // "Neta: Good morning, dear!"
			.ShowDialog(0x0304) // "Neta: Don't forget, you start at the weapon shop today!"
			.ShowDialog(0x0305) // "Taloon: I'll make us proud, Neta!"
			.SetFlag(FlagLeftHome)
			.End()
			// Already left once
			.ShowDialog(0x0306) // "Neta: Have a good day at work!"
			.End()
			.Build();
	}

	/// <summary>
	/// First day at the weapon shop - shop owner introduces work.
	/// </summary>
	public static EventScript BuildFirstDayShopScript() {
		return new EventScriptBuilder(FirstDayShop)
			.WithName("First Day Shop")
			.WithCategory(ScriptCategory.Shop)
			.ForChapter(2)
			.CheckFlag(FlagShopTutorial, 0x0020)
			// First time
			.ShowDialog(0x0310) // "Shop Owner: Ah, you must be Taloon!"
			.ShowDialog(0x0311) // "Shop Owner: Let me show you how we run things here."
			.ShowDialog(0x0312) // "Shop Owner: When customers come in, serve them well!"
			.JumpSubroutine(ShopTutorialBuy)
			.JumpSubroutine(ShopTutorialSell)
			.SetFlag(FlagShopTutorial)
			.ShowDialog(0x0313) // "Shop Owner: Now you're ready. Good luck!"
			.End()
			// Already trained
			.ShowDialog(0x0314) // "Shop Owner: Get to work, Taloon!"
			.End()
			.Build();
	}

	/// <summary>
	/// Shop tutorial - how to buy items from customers.
	/// </summary>
	public static EventScript BuildShopTutorialBuyScript() {
		return new EventScriptBuilder(ShopTutorialBuy)
			.WithName("Shop Tutorial Buy")
			.WithCategory(ScriptCategory.Shop)
			.ForChapter(2)
			.ShowDialog(0x0320) // "Shop Owner: When someone wants to sell..."
			.ShowDialog(0x0321) // "Shop Owner: Offer a fair price based on condition."
			.ShowDialog(0x0322) // "Shop Owner: Buy low to make profit later!"
			.Return()
			.Build();
	}

	/// <summary>
	/// Shop tutorial - how to sell items to customers.
	/// </summary>
	public static EventScript BuildShopTutorialSellScript() {
		return new EventScriptBuilder(ShopTutorialSell)
			.WithName("Shop Tutorial Sell")
			.WithCategory(ScriptCategory.Shop)
			.ForChapter(2)
			.ShowDialog(0x0323) // "Shop Owner: When someone wants to buy..."
			.ShowDialog(0x0324) // "Shop Owner: Know your inventory and prices!"
			.ShowDialog(0x0325) // "Shop Owner: Satisfied customers return!"
			.Return()
			.Build();
	}

	/// <summary>
	/// Earning enough gold to buy the iron safe.
	/// </summary>
	public static EventScript BuildEarnIronSafeScript() {
		return new EventScriptBuilder(EarnIronSafe)
			.WithName("Earn Iron Safe")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(2)
			.CheckFlag(FlagIronSafe, 0x0010)
			// First time
			.ShowDialog(0x0330) // "Shop Owner: You've earned enough for an iron safe!"
			.ShowDialog(0x0331) // "Shop Owner: This will keep your savings secure."
			.GiveItem(ItemIronSafe)
			.SetFlag(FlagIronSafe)
			.PlaySound(0x30) // Item get sound
			.ShowDialog(0x0332) // "Taloon received the Iron Safe!"
			.End()
			// Already have
			.ShowDialog(0x0333) // "Keep saving up, Taloon!"
			.End()
			.Build();
	}

	/// <summary>
	/// Old man tells about the cave to the east.
	/// </summary>
	public static EventScript BuildOldManCaveScript() {
		return new EventScriptBuilder(OldManCave)
			.WithName("Old Man Cave")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(2)
			.CheckFlag(FlagOldManCave, 0x0010)
			// First time
			.ShowDialog(0x0340) // "Old Man: You look like a capable merchant..."
			.ShowDialog(0x0341) // "Old Man: There's a cave east of town."
			.ShowDialog(0x0342) // "Old Man: Monsters guard valuable treasures there!"
			.SetFlag(FlagOldManCave)
			.End()
			// Repeat
			.ShowDialog(0x0343) // "Old Man: Found anything good in that cave?"
			.End()
			.Build();
	}

	/// <summary>
	/// First cave exploration - entering the dungeon.
	/// </summary>
	public static EventScript BuildCaveFirstScript() {
		return new EventScriptBuilder(CaveFirst)
			.WithName("Cave First")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(2)
			.PlayMusic(MusicDungeon)
			.ShowDialog(0x0350) // "This cave is dark and dangerous..."
			.ShowDialog(0x0351) // "But a merchant must take risks for profit!"
			.End()
			.Build();
	}

	/// <summary>
	/// Finding the Steel Broadsword in the cave.
	/// </summary>
	public static EventScript BuildFindSteelSwordScript() {
		return new EventScriptBuilder(FindSteelSword)
			.WithName("Find Steel Sword")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(2)
			.CheckFlag(FlagFoundSword, 0x0010)
			// First time
			.ShowDialog(0x0360) // "Taloon found a Steel Broadsword!"
			.GiveItem(ItemSteelSword)
			.SetFlag(FlagFoundSword)
			.PlaySound(0x30) // Item get sound
			.ShowDialog(0x0361) // "This will sell for a good price!"
			.End()
			// Already found
			.ShowDialog(0x0362) // "The chest is empty."
			.End()
			.Build();
	}

	/// <summary>
	/// Returning the sword to the shop for sale.
	/// </summary>
	public static EventScript BuildReturnSwordScript() {
		return new EventScriptBuilder(ReturnSword)
			.WithName("Return Sword")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(2)
			.CheckItem(ItemSteelSword, 0x0010)
			// Has sword
			.ShowDialog(0x0370) // "Shop Owner: That's a fine sword, Taloon!"
			.ShowDialog(0x0371) // "Shop Owner: I'll give you a good price for it."
			.TakeItem(ItemSteelSword)
			.GiveGold(1500)
			.PlaySound(0x31) // Coins sound
			.ShowDialog(0x0372) // "Taloon received 1500 gold!"
			.End()
			// No sword
			.ShowDialog(0x0373) // "Shop Owner: Find anything valuable?"
			.End()
			.Build();
	}

	/// <summary>
	/// Discovery of Fox Village.
	/// </summary>
	public static EventScript BuildFoxVillageScript() {
		return new EventScriptBuilder(FoxVillage)
			.WithName("Fox Village")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(2)
			.CheckFlag(FlagFoxVillage, 0x0010)
			// First time
			.ShowDialog(0x0380) // "What's this? A village of foxes?"
			.ShowDialog(0x0381) // "Fox: Don't be alarmed, merchant!"
			.ShowDialog(0x0382) // "Fox: We foxes have goods to trade too!"
			.SetFlag(FlagFoxVillage)
			.End()
			// Return visit
			.ShowDialog(0x0383) // "Fox: Welcome back, merchant!"
			.End()
			.Build();
	}

	/// <summary>
	/// Prince Reed's request for the Silver Statuette.
	/// </summary>
	public static EventScript BuildPrinceReedScript() {
		return new EventScriptBuilder(PrinceReed)
			.WithName("Prince Reed")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(2)
			.CheckFlag(FlagPrinceReed, 0x0010)
			// First time
			.ShowDialog(0x0390) // "Prince Reed: You're a merchant, yes?"
			.ShowDialog(0x0391) // "Prince Reed: I need help finding something..."
			.ShowDialog(0x0392) // "Prince Reed: A Silver Statuette in a cave nearby."
			.ShowDialog(0x0393) // "Prince Reed: Bring it to me and I'll reward you handsomely!"
			.SetFlag(FlagPrinceReed)
			.End()
			// Already talked
			.CheckItem(ItemSilverStatuette, 0x0020)
			// Has statuette - jump to deliver
			.Jump(DeliverStatuette)
			// No statuette
			.ShowDialog(0x0394) // "Prince Reed: Still looking for that statuette?"
			.End()
			.Build();
	}

	/// <summary>
	/// Cave of the Silver Statuette - finding the quest item.
	/// </summary>
	public static EventScript BuildSilverStatuetteCaveScript() {
		return new EventScriptBuilder(SilverStatuetteCave)
			.WithName("Silver Statuette Cave")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(2)
			.CheckFlag(FlagStatuette, 0x0010)
			// First time
			.ShowDialog(0x03A0) // "There it is! The Silver Statuette!"
			.GiveItem(ItemSilverStatuette)
			.PlaySound(0x30)
			.ShowDialog(0x03A1) // "Taloon received the Silver Statuette!"
			.End()
			// Already got it
			.ShowDialog(0x03A2) // "The pedestal is empty."
			.End()
			.Build();
	}

	/// <summary>
	/// Delivering the Silver Statuette to Prince Reed.
	/// </summary>
	public static EventScript BuildDeliverStatuetteScript() {
		return new EventScriptBuilder(DeliverStatuette)
			.WithName("Deliver Statuette")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(2)
			.ShowDialog(0x03B0) // "Prince Reed: You found it! Excellent!"
			.TakeItem(ItemSilverStatuette)
			.SetFlag(FlagStatuette)
			.GiveGold(5000)
			.PlaySound(0x31)
			.ShowDialog(0x03B1) // "Prince Reed: Here's your reward - 5000 gold!"
			.ShowDialog(0x03B2) // "Prince Reed: You're quite the merchant!"
			.End()
			.Build();
	}

	/// <summary>
	/// Earning enough to open your own shop.
	/// </summary>
	public static EventScript BuildOpenOwnShopScript() {
		return new EventScriptBuilder(OpenOwnShop)
			.WithName("Open Own Shop")
			.WithCategory(ScriptCategory.Shop)
			.ForChapter(2)
			.CheckFlag(FlagOwnShop, 0x0010)
			// First time
			.ShowDialog(0x03C0) // "Neta: Taloon, you've saved so much gold!"
			.ShowDialog(0x03C1) // "Neta: You could finally open your own shop!"
			.ShowDialog(0x03C2) // "Taloon: Yes! But first, I must go to Endor!"
			.ShowDialog(0x03C3) // "Taloon: There's a bigger market there!"
			.SetFlag(FlagOwnShop)
			.End()
			// After
			.ShowDialog(0x03C4) // "Neta: Be careful on your journey to Endor!"
			.End()
			.Build();
	}

	/// <summary>
	/// Getting ship passage to Endor.
	/// </summary>
	public static EventScript BuildShipPassageScript() {
		return new EventScriptBuilder(ShipPassage)
			.WithName("Ship Passage")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(2)
			.CheckFlag(FlagShipPassage, 0x0010)
			// First time
			.ShowDialog(0x03D0) // "Captain: Passage to Endor, eh?"
			.ShowDialog(0x03D1) // "Captain: That'll be 200 gold."
			.TakeGold(200)
			.GiveItem(ItemShipTicket)
			.SetFlag(FlagShipPassage)
			.ShowDialog(0x03D2) // "Captain: Welcome aboard!"
			.End()
			// Already have ticket
			.ShowDialog(0x03D3) // "Captain: We set sail soon!"
			.End()
			.Build();
	}

	/// <summary>
	/// Arriving at Endor.
	/// </summary>
	public static EventScript BuildArriveEndorScript() {
		return new EventScriptBuilder(ArriveEndor)
			.WithName("Arrive Endor")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(2)
			.TakeItem(ItemShipTicket)
			.FadeOut()
			.Warp(0x28, 10, 10) // Endor map
			.PlayMusic(MusicOverworld)
			.FadeIn()
			.ShowDialog(0x03E0) // "So this is Endor! The great trading city!"
			.ShowDialog(0x03E1) // "Here I'll make my fortune!"
			.End()
			.Build();
	}

	/// <summary>
	/// Meeting the arms dealer in Endor.
	/// </summary>
	public static EventScript BuildMeetArmsDealerScript() {
		return new EventScriptBuilder(MeetArmsDealer)
			.WithName("Meet Arms Dealer")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(2)
			.CheckFlag(FlagArmsDealer, 0x0010)
			// First time
			.ShowDialog(0x03F0) // "Arms Dealer: You're the merchant from Lakanaba?"
			.ShowDialog(0x03F1) // "Arms Dealer: I've heard good things about you!"
			.ShowDialog(0x03F2) // "Arms Dealer: There's a tunnel being built..."
			.ShowDialog(0x03F3) // "Arms Dealer: Help fund it and you'll be rich!"
			.SetFlag(FlagArmsDealer)
			.End()
			// After
			.ShowDialog(0x03F4) // "Arms Dealer: The tunnel is coming along!"
			.End()
			.Build();
	}

	/// <summary>
	/// Tunnel construction started.
	/// </summary>
	public static EventScript BuildTunnelStartScript() {
		return new EventScriptBuilder(TunnelStart)
			.WithName("Tunnel Start")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(2)
			.CheckFlag(FlagTunnelStart, 0x0010)
			// First time
			.ShowDialog(0x0400) // "Worker: We've started digging the tunnel!"
			.ShowDialog(0x0401) // "Worker: It'll connect the two lands!"
			.SetFlag(FlagTunnelStart)
			.End()
			// After
			.ShowDialog(0x0402) // "Worker: Keep working hard!"
			.End()
			.Build();
	}

	/// <summary>
	/// Tunnel construction complete.
	/// </summary>
	public static EventScript BuildTunnelCompleteScript() {
		return new EventScriptBuilder(TunnelComplete)
			.WithName("Tunnel Complete")
			.WithCategory(ScriptCategory.Story)
			.ForChapter(2)
			.CheckFlag(FlagTunnelDone, 0x0010)
			// First time
			.PlayMusic(MusicVictory)
			.ShowDialog(0x0410) // "Worker: The tunnel is COMPLETE!"
			.ShowDialog(0x0411) // "Worker: Trade routes are now open!"
			.SetFlag(FlagTunnelDone)
			.End()
			// After
			.ShowDialog(0x0412) // "The tunnel buzzes with merchants!"
			.End()
			.Build();
	}

	/// <summary>
	/// Chapter 3 completion script.
	/// </summary>
	public static EventScript BuildChapterCompleteScript() {
		return new EventScriptBuilder(ChapterComplete)
			.WithName("Chapter 3 Complete")
			.WithCategory(ScriptCategory.Cutscene)
			.ForChapter(2)
			.FadeOut()
			.ShowDialog(0x0420) // "Taloon's dream of becoming a great merchant..."
			.ShowDialog(0x0421) // "...was only beginning."
			.ShowDialog(0x0422) // "But destiny had other plans for this merchant..."
			.SetChapter(3) // Move to Chapter 4
			.End()
			.Build();
	}

	// ============================================================
	// Service Scripts
	// ============================================================

	/// <summary>
	/// Lakanaba weapon shop where Taloon works.
	/// </summary>
	public static EventScript BuildLakanabaWeaponShopScript() {
		return new EventScriptBuilder(LakanabaWeaponShop)
			.WithName("Lakanaba Weapon Shop")
			.WithCategory(ScriptCategory.Shop)
			.ForChapter(2)
			.ShowDialog(0x0430) // "Welcome to our weapon shop!"
			.OpenShop(0x10)
			.End()
			.Build();
	}

	/// <summary>
	/// Lakanaba item shop.
	/// </summary>
	public static EventScript BuildLakanabaItemShopScript() {
		return new EventScriptBuilder(LakanabaItemShop)
			.WithName("Lakanaba Item Shop")
			.WithCategory(ScriptCategory.Shop)
			.ForChapter(2)
			.ShowDialog(0x0431) // "Welcome to the item shop!"
			.OpenShop(0x11)
			.End()
			.Build();
	}

	/// <summary>
	/// Lakanaba inn.
	/// </summary>
	public static EventScript BuildLakanabaInnScript() {
		return new EventScriptBuilder(LakanabaInn)
			.WithName("Lakanaba Inn")
			.WithCategory(ScriptCategory.Inn)
			.ForChapter(2)
			.ShowDialog(0x0432) // "Welcome to the inn!"
			.OpenInn(0x10, 6)
			.End()
			.Build();
	}

	/// <summary>
	/// Lakanaba church.
	/// </summary>
	public static EventScript BuildLakanabaChurchScript() {
		return new EventScriptBuilder(LakanabaChurch)
			.WithName("Lakanaba Church")
			.WithCategory(ScriptCategory.NPC)
			.ForChapter(2)
			.ShowDialog(0x0433) // "Welcome to the church."
			.OpenChurch()
			.End()
			.Build();
	}

	/// <summary>
	/// Foxville shop (run by foxes).
	/// </summary>
	public static EventScript BuildFoxvilleShopScript() {
		return new EventScriptBuilder(FoxvilleShop)
			.WithName("Foxville Shop")
			.WithCategory(ScriptCategory.Shop)
			.ForChapter(2)
			.ShowDialog(0x0434) // "Fox: Browse our wares, merchant!"
			.OpenShop(0x12)
			.End()
			.Build();
	}

	/// <summary>
	/// Bonmalmo shop.
	/// </summary>
	public static EventScript BuildBonmalmoShopScript() {
		return new EventScriptBuilder(BonmalmoShop)
			.WithName("Bonmalmo Shop")
			.WithCategory(ScriptCategory.Shop)
			.ForChapter(2)
			.ShowDialog(0x0435) // "Welcome to Bonmalmo's shop!"
			.OpenShop(0x13)
			.End()
			.Build();
	}

	/// <summary>
	/// Bonmalmo inn.
	/// </summary>
	public static EventScript BuildBonmalmoInnScript() {
		return new EventScriptBuilder(BonmalmoInn)
			.WithName("Bonmalmo Inn")
			.WithCategory(ScriptCategory.Inn)
			.ForChapter(2)
			.ShowDialog(0x0436) // "Rest here, traveler?"
			.OpenInn(0x11, 10)
			.End()
			.Build();
	}

	/// <summary>
	/// Endor weapon shop.
	/// </summary>
	public static EventScript BuildEndorWeaponShopScript() {
		return new EventScriptBuilder(EndorWeaponShop)
			.WithName("Endor Weapon Shop")
			.WithCategory(ScriptCategory.Shop)
			.ForChapter(2)
			.ShowDialog(0x0437) // "The finest weapons in Endor!"
			.OpenShop(0x14)
			.End()
			.Build();
	}

	/// <summary>
	/// Endor inn.
	/// </summary>
	public static EventScript BuildEndorInnScript() {
		return new EventScriptBuilder(EndorInn)
			.WithName("Endor Inn")
			.WithCategory(ScriptCategory.Inn)
			.ForChapter(2)
			.ShowDialog(0x0438) // "Welcome to Endor's inn!"
			.OpenInn(0x12, 20)
			.End()
			.Build();
	}

	// ============================================================
	// Aggregate Methods
	// ============================================================

	/// <summary>
	/// Get all Chapter 3 scripts.
	/// </summary>
	public static EventScript[] GetAllScripts() {
		return [
			// Story scripts (21)
			BuildIntroScript(),
			BuildMorningHomeScript(),
			BuildFirstDayShopScript(),
			BuildShopTutorialBuyScript(),
			BuildShopTutorialSellScript(),
			BuildEarnIronSafeScript(),
			BuildOldManCaveScript(),
			BuildCaveFirstScript(),
			BuildFindSteelSwordScript(),
			BuildReturnSwordScript(),
			BuildFoxVillageScript(),
			BuildPrinceReedScript(),
			BuildSilverStatuetteCaveScript(),
			BuildDeliverStatuetteScript(),
			BuildOpenOwnShopScript(),
			BuildShipPassageScript(),
			BuildArriveEndorScript(),
			BuildMeetArmsDealerScript(),
			BuildTunnelStartScript(),
			BuildTunnelCompleteScript(),
			BuildChapterCompleteScript(),
			// Service scripts (9)
			BuildLakanabaWeaponShopScript(),
			BuildLakanabaItemShopScript(),
			BuildLakanabaInnScript(),
			BuildLakanabaChurchScript(),
			BuildFoxvilleShopScript(),
			BuildBonmalmoShopScript(),
			BuildBonmalmoInnScript(),
			BuildEndorWeaponShopScript(),
			BuildEndorInnScript()
		];
	}
}

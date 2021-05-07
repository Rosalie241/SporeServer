using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.SporeTypes
{
    public class SporeAchievements
    {
        public static SporeAchievement[] Achievements =
        {
            new SporeAchievement()
            {
                // filename: alg-photographer
                Id = 0xb4c2a66b,
                Name = "Photographer",
                Description = "Send a photo or video to a friend from Test Drive mode",
                // filename: photographer
                UnlockedFileNameHash = "0x80e800d8",
                // filename: photographer_disabled
                LockedFileNameHash = "0x847f9577",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: alg-unlockcrg
                Id = 0x26b05664,
                Name = "Creature stage unlocked",
                Description = "Play enough of the Cell stage to unlock the Creature stage",
                // filename: CreatureGame
                UnlockedFileNameHash = "0xad56080c",
                // filename: CreatureGame_disabled
                LockedFileNameHash = "0xefa5e5fb",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: alg-unlocktrg
                Id = 0x24947c07,
                Name = "Tribe stage unlocked",
                Description = "Play enough of the Creature stage to unlock the Tribe stage",
                // filename: TribeGame
                UnlockedFileNameHash = "0xf71fa311",
                // filename: tribegame_disabled
                LockedFileNameHash = "0x084af674",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: alg-unlockcvg
                Id = 0x2ab05c28,
                Name = "Civilization stage unlocked",
                Description = "Play enough of the Tribe stage to unlock the Civilization stage",
                // filename: CivGame
                UnlockedFileNameHash = "0xbeb528cb",
                // filename: civgame_disabled
                LockedFileNameHash = "0xf4883ab6",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: alg-unlockspg
                Id = 0x2887b6de,
                Name = "Space stage unlocked",
                Description = "Play enough of the Civilization stage to unlock the Space stage",
                // filename: SpaceGame
                UnlockedFileNameHash = "0x2db6dad3",
                // filename: spacegame_disabled
                LockedFileNameHash = "0x8452ce1e",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: clg-landfall
                Id = 0xde776d90,
                Name = "Landfall",
                Description = "Finish the Cell stage and clamber onto the planet's surface",
                // filename: landfall
                UnlockedFileNameHash = "0xd3ddae7d",
                // filename: landfall_disabled
                LockedFileNameHash = "0x807fe0d0",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: clg-completist
                Id = 0x83932384,
                Name = "Completist",
                Description = "Unlock all the parts in the Cell stage",
                // filename: completist
                UnlockedFileNameHash = "0xaa4bd7a5",
                // filename: completist_disabled
                LockedFileNameHash = "0x3655c818",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: crg-evolver
                Id = 0x8f5e1285,
                Name = "Evolver",
                Description = "Finish Creature stage",
                // filename: 0xE8491003
                UnlockedFileNameHash = "0xe8491003",
                // filename: 0x5D4ADA6E
                LockedFileNameHash = "0x5d4ada6e",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: trg-founder
                Id = 0x4a7480ac,
                Name = "Founder",
                Description = "Complete the Tribal stage and build a city",
                // filename: founder
                UnlockedFileNameHash = "0xba87e07e",
                // filename: founder_disabled
                LockedFileNameHash = "0x59a44a95",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: trg-vicious
                Id = 0x8aa440ab,
                Name = "Vicious",
                Description = "Kill all members of all 5 opposing tribes and raze their villages",
                // filename: vicious
                UnlockedFileNameHash = "0x47702a15",
                // filename: vicious_disabled
                LockedFileNameHash = "0x65689328",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: trg-thehorde
                Id = 0xa784ec4a,
                Name = "Ergonomically Terrific!",
                Description = "Complete the Tribal stage in less than an hour",
                // filename: thehorde
                UnlockedFileNameHash = "0x13a3edd8",
                // filename: thehorde_disabled
                LockedFileNameHash = "0x66a18c77",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: alg-automotiveengineer
                Id = 0xadda228a,
                Name = "Automotive Engineer",
                Description = "Make and publish 50 vehicles",
                // filename: automotiveengineer
                UnlockedFileNameHash = "0xb0b4c151",
                // filename: automotiveengineer_disabled
                LockedFileNameHash = "0x486952b4",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: alg-sporefan
                Id = 0x54cf0c2c,
                Name = "Spore Fan",
                Description = "Spend 50 hours in your Spore galaxy",
                // filename: sporefan
                UnlockedFileNameHash = "0xe52ba3c3",
                // filename: sporefan_disabled
                LockedFileNameHash = "0xc2b09e2e",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: alg-risingstar
                Id = 0x6baecaf4,
                Name = "Rising Star",
                Description = "Have 5 different Sporecasts subscribed to by at least 10 people",
                // filename: risingstar
                UnlockedFileNameHash = "0x76d3b5e3",
                // filename: risingstar_disabled
                LockedFileNameHash = "0x9b913a8e",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: alg-frontpagenews
                Id = 0x1802c08f,
                Name = "Front Page News",
                Description = "Have one of your creations or Sporecasts featured on www.spore.com",
                // filename: frontpagenews
                UnlockedFileNameHash = "0x693a7e9a",
                // filename: frontpagenews_disabled
                LockedFileNameHash = "0xf916aac1",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: alg-architect
                Id = 0x71c5a637,
                Name = "Architect",
                Description = "Create and share 50 buildings",
                // filename: architect
                UnlockedFileNameHash = "0xe05d55ca",
                // filename: architect_disabled
                LockedFileNameHash = "0x7a9ed6d1",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: alg-steeltribe
                Id = 0x8d82fac1,
                Name = "Steel Tribe",
                Description = "Complete Tribe stage on Hard setting",
                // filename: steeltribe
                UnlockedFileNameHash = "0x4ef0a412",
                // filename: steeltribe_disabled
                LockedFileNameHash = "0xb16efd79",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: alg-biologist
                Id = 0x2e3fc8f2,
                Name = "Biologist",
                Description = "Make and publish 100 creatures",
                // filename: biologist
                UnlockedFileNameHash = "0x25715b67",
                // filename: biologist_disabled
                LockedFileNameHash = "0x5c3a7882",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: alg-ironcreature
                Id = 0x24545cdf,
                Name = "Iron Creature",
                Description = "Complete Creature stage on Hard setting",
                // filename: ironcreature
                UnlockedFileNameHash = "0x11cd0b94",
                // filename: ironcreature_disabled
                LockedFileNameHash = "0xcb7e6d43",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: alg-sporeaddict
                Id = 0x0257c7f6,
                Name = "Spore Addict",
                Description = "Spend 100 hours in your Spore galaxy",
                // filename: sporeaddict
                UnlockedFileNameHash = "0xaffe24cf",
                // filename: sporeaddict_disabled
                LockedFileNameHash = "0x82681c8a",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: alg-creator
                Id = 0xbaebb1d6,
                Name = "Creator",
                Description = "Spend 50 hours in the creators",
                // filename: creator
                UnlockedFileNameHash = "0xd004e927",
                // filename: creator_disabled
                LockedFileNameHash = "0x82575042",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: alg-galacticgod
                Id = 0xaf2819b8,
                Name = "Galactic God",
                Description = "Evolve from a cell to a space traveler in one continuous game",
                // filename: galacticgod
                UnlockedFileNameHash = "0xe73c15c1",
                // filename: galacticgod_disabled
                LockedFileNameHash = "0xbbc8bb04",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: alg-titaniumcivilization
                Id = 0xa7b1c920,
                Name = "Adamantium Civilization",
                Description = "Complete Civilization stage on Hard setting",
                // filename: titaniumcivilization
                UnlockedFileNameHash = "0x9787bbb3",
                // filename: titaniumcivilization_disabled
                LockedFileNameHash = "0x7fe02b3e",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: alg-dejavu
                Id = 0xbde3bf2f,
                Name = "Deja Vu",
                Description = "Discover something you created in another game",
                // filename: dejavu
                UnlockedFileNameHash = "0xd3ae1498",
                // filename: dejavu_disabled
                LockedFileNameHash = "0x23237737",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: alg-aluminumcell
                Id = 0x20b852b8,
                Name = "Aluminum Cell",
                Description = "Complete Cell stage on Hard setting",
                // filename: aluminumcell
                UnlockedFileNameHash = "0x213ecac3",
                // filename: aluminumcell_disabled
                LockedFileNameHash = "0x1e1ea32e",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: alg-universeinabox
                Id = 0xd082675a,
                Name = "Universe In A Box",
                Description = "Play in every stage and every creator",
                // filename: universeinabox
                UnlockedFileNameHash = "0xbd0dbce5",
                // filename: universeinabox_disabled
                LockedFileNameHash = "0x7fd8fe58",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: alg-socialengineer
                Id = 0xef7cd950,
                Name = "Social Engineer",
                Description = "Make 5 Sporecasts of 50 creations or more",
                // filename: socialengineer
                UnlockedFileNameHash = "0x478c04bf",
                // filename: socialengineer_disabled
                LockedFileNameHash = "0x0cecedda",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: clg-pacifist
                Id = 0x662732af,
                Name = "Pacifist",
                Description = "Finish the Cell stage without killing another creature",
                // filename: pacifist
                UnlockedFileNameHash = "0x6cde814e",
                // filename: pacifist_disabled
                LockedFileNameHash = "0xb58a0745",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: clg-celladdict
                Id = 0x345fbd9f,
                Name = "Cell Addict",
                Description = "Finish the Cell stage 25 times",
                // filename: celladdict
                UnlockedFileNameHash = "0x7a05b9ce",
                // filename: celladdict_disabled
                LockedFileNameHash = "0x0024a8c5",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: clg-speedfreak
                Id = 0x0d0d8ca6,
                Name = "Speedfreak",
                Description = "Finish the Cell stage in under 8 minutes",
                // filename: speedfreak
                UnlockedFileNameHash = "0xf8b57237",
                // filename: speedfreak_disabled
                LockedFileNameHash = "0xefcf2672",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: crg-flightofthebumblebee
                Id = 0x04913ee1,
                Name = "Flight of the Bumblebee",
                Description = "Fly for over 200 meters without touching the ground",
                // filename: flightofthebumblebee
                UnlockedFileNameHash = "0x45a531ca",
                // filename: flightofthebumblebee_disabled
                LockedFileNameHash = "0x62d4aad1",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: crg-foe
                Id = 0x94f5e078,
                Name = "Foe",
                Description = "Extinct at least 20 other species in Creature stage",
                // filename: foe
                UnlockedFileNameHash = "0x408f5e19",
                // filename: foe_disabled
                LockedFileNameHash = "0x9215d9fc",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: crg-friend
                Id = 0x19d844e6,
                Name = "Everyone's BFF",
                Description = "Finish the Creature stage by befriending at least 20 other species",
                // filename: friend
                UnlockedFileNameHash = "0xb457a091",
                // filename: friend_disabled
                LockedFileNameHash = "0x6e9096f4",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: crg-bestial
                Id = 0x8baf77de,
                Name = "Bestial",
                Description = "Play the Creature stage 10 times",
                // filename: bestial
                UnlockedFileNameHash = "0x3525c9b7",
                // filename: bestial_disabled
                LockedFileNameHash = "0xac1234f2",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: crg-speeddemon
                Id = 0x838a3cf4,
                Name = "Speed Demon",
                Description = "Finish Creature stage within an hour",
                // filename: leopard
                UnlockedFileNameHash = "0xcada42a8",
                // filename: leopard_disabled
                LockedFileNameHash = "0x48169de7",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: crg-maxpower
                Id = 0x7055b10b,
                Name = "Max Power",
                Description = "Build a creature with maximum stats in at least 4 abilities in Creature stage",
                // filename: maxpower
                UnlockedFileNameHash = "0xf57f0298",
                // filename: maxpower_disabled
                LockedFileNameHash = "0xdef56137",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: crg-epickiller
                Id = 0x343100cc,
                Name = "Epic Killer",
                Description = "Kill an epic in Creature stage",
                // filename: 0x9675ABA3
                UnlockedFileNameHash = "0x9675aba3",
                // filename: 0x5D98734E
                LockedFileNameHash = "0x5d98734e",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: crg-socialite
                Id = 0xd35d68dd,
                Name = "Socialite",
                Description = "Meet 200 creatures made by other players",
                // filename: crgSocialite
                UnlockedFileNameHash = "0xd5017cc0",
                // filename: crgSocialite_disabled
                LockedFileNameHash = "0xcbbe16ff",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: crg-devourer
                Id = 0xe25acf0e,
                Name = "Devourer",
                Description = "Eat 50 different species across any number of games",
                // filename: 0xF8B7876F
                UnlockedFileNameHash = "0xf8b7876f",
                // filename: 0x9DFF13EA
                LockedFileNameHash = "0x9dff13ea",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: crg-immortal
                Id = 0xba577381,
                Name = "Survivor",
                Description = "Finish Creature stage without dying",
                // filename: survivor
                UnlockedFileNameHash = "0x7b782b6b",
                // filename: survivor_disabled
                LockedFileNameHash = "0x3a9ad716",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: crg-villagepeople
                Id = 0xe9c1e353,
                Name = "Village Folks",
                Description = "Have three posse members from different species",
                // filename: villagepeople
                UnlockedFileNameHash = "0x27481276",
                // filename: villagepeople_disabled
                LockedFileNameHash = "0x0ddfb60d",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: trg-watchfulparent
                Id = 0xb598f829,
                Name = "Watchful Parent",
                Description = "Complete the Tribal stage without the death of a single tribe member",
                // filename: watchfulparent
                UnlockedFileNameHash = "0x35aafd23",
                // filename: watchfulparent_disabled
                LockedFileNameHash = "0xe654cfce",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: trg-socialite
                Id = 0x7859c5da,
                Name = "Tribal Socialite",
                Description = "Convert all 5 opposing tribes to your belief system",
                // filename: TrgSocialite
                UnlockedFileNameHash = "0x2f23a431",
                // filename: TrgSocialite_disabled
                LockedFileNameHash = "0xfc9cd454",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: trg-tribal
                Id = 0xa6c22573,
                Name = "Tribal",
                Description = "Complete the Tribal stage 10 times",
                // filename: tribal
                UnlockedFileNameHash = "0x98eeb4f9",
                // filename: tribal_disabled
                LockedFileNameHash = "0x7006999c",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: trg-domesticbliss
                Id = 0x9073fca4,
                Name = "Domestic Bliss",
                Description = "Domesticate and farm three different species",
                // filename: domesticbliss
                UnlockedFileNameHash = "0x0214c826",
                // filename: domesticbliss_disabled
                LockedFileNameHash = "0x1212395d",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: cvg-rollingthunder
                Id = 0x6a5c80e9,
                Name = "Rolling Thunder",
                Description = "Complete the Civilization stage in less than an hour",
                // filename: rollingthunder
                UnlockedFileNameHash = "0x2fda8986",
                // filename: rollingthunder_disabled
                LockedFileNameHash = "0x1f4cb77d",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: cvg-starman
                Id = 0x2b9868da,
                Name = "Starman",
                Description = "Conquer all the other cities in the Civilization stage and launch your first space vessel",
                // filename: starman
                UnlockedFileNameHash = "0x96ba4aff",
                // filename: starman_disabled
                LockedFileNameHash = "0x61e4851a",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: cvg-relentless
                Id = 0xb2352dc5,
                Name = "Relentless",
                Description = "Complete the Civilization stage 10 times",
                // filename: relentless
                UnlockedFileNameHash = "0x718bfb2a",
                // filename: relentless_disabled
                LockedFileNameHash = "0x33a02571",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: cvg-economist
                Id = 0xc15fda9b,
                Name = "Economist",
                Description = "Finish the Civilization stage with more than 8 economic cities",
                // filename: economist
                UnlockedFileNameHash = "0xb793bdc6",
                // filename: economist_disabled
                LockedFileNameHash = "0x48d0b3bd",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: cvg-militarystrongman
                Id = 0x256ed7f6,
                Name = "Military Strongman",
                Description = "Finish the Civilization stage with more than 8 military cities",
                // filename: 0x8A3C22E1
                UnlockedFileNameHash = "0x8a3c22e1",
                // filename: 0xE26A43E4
                LockedFileNameHash = "0xe26a43e4",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: cvg-spicehorder
                Id = 0x3e7a7f8c,
                Name = "Spice Hoarder",
                Description = "Control every resource node on the planet simultaneously",
                // filename: spicehorder
                UnlockedFileNameHash = "0x2c348761",
                // filename: spicehorder_disabled
                LockedFileNameHash = "0xb091a964",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: cvg-ghettoblaster
                Id = 0x1f9c52ae,
                Name = "Ghetto Blaster",
                Description = "Create 10 Anthems",
                // filename: ghettoblaster
                UnlockedFileNameHash = "0xb464cd5f",
                // filename: ghettoblaster_disabled
                LockedFileNameHash = "0x29c638ba",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: cvg-missionary
                Id = 0x78d05a36,
                Name = "Missionary",
                Description = "Finish the Civilization stage with more than 8 religious cities",
                // filename: missionary
                UnlockedFileNameHash = "0x9eb159e1",
                // filename: missionary_disabled
                LockedFileNameHash = "0xaeed78e4",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-archetypewanderer
                Id = 0xae94ffe3,
                Name = "Wanderer Passion",
                Description = "Play as a Wanderer",
                // filename: WandererPassion
                UnlockedFileNameHash = "0x74465d52",
                // filename: WandererPassion_disabled
                LockedFileNameHash = "0xef62a6b9",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-archetypetrader
                Id = 0x23752db5,
                Name = "Trader Passion",
                Description = "Play as a Trader",
                // filename: TraderPassion
                UnlockedFileNameHash = "0x824261e8",
                // filename: TraderPassion_disabled
                LockedFileNameHash = "0xe655a727",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-mb10wanderer
                Id = 0xf48bdca4,
                Name = "Wanderer Hero",
                Description = "Achieve Master Badge Level 10 as a Wanderer",
                // filename: wandererhero
                UnlockedFileNameHash = "0xa059e42b",
                // filename: wandererhero_disabled
                LockedFileNameHash = "0x1d70dad6",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-mb10scientist
                Id = 0x13ebce3c,
                Name = "Scientist Hero",
                Description = "Achieve Master Badge Level 10 as a Scientist",
                // filename: scientisthero
                UnlockedFileNameHash = "0xb4664d81",
                // filename: scientisthero_disabled
                LockedFileNameHash = "0x72af4bc4",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-archetypebard
                Id = 0x7d7b59e8,
                Name = "Bard Passion",
                Description = "Play as a Bard",
                // filename: BardPassion
                UnlockedFileNameHash = "0xd11ddc5f",
                // filename: BardPassion_disabled
                LockedFileNameHash = "0x70af75ba",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-mb10zealot
                Id = 0xad1f3843,
                Name = "Zealot Hero",
                Description = "Achieve Master Badge Level 10 as a Zealot",
                // filename: zealothero
                UnlockedFileNameHash = "0xb2f67010",
                // filename: zealothero_disabled
                LockedFileNameHash = "0x552f38ef",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-conquistador
                Id = 0xd3f14a26,
                Name = "Conquistador",
                Description = "Capture 15 Star Systems",
                // filename: conquistador
                UnlockedFileNameHash = "0x00657f63",
                // filename: conquistador_disabled
                LockedFileNameHash = "0xb4137f0e",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-eaglescout
                Id = 0xb379fbce,
                Name = "Maxis Scout",
                Description = "Earn 100 badges in the Space stage",
                // filename: eaglescout
                UnlockedFileNameHash = "0x01973223",
                // filename: eaglescout_disabled
                LockedFileNameHash = "0x874b1ece",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-planetkiller
                Id = 0x5c54d48f,
                Name = "Quietus Star",
                Description = "Destroy 20 planets",
                // filename: Deathstar
                UnlockedFileNameHash = "0x75637fa5",
                // filename: Deathstar_disabled
                LockedFileNameHash = "0xfbca4018",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-splitpersonality
                Id = 0x4a559e78,
                Name = "Split Personality",
                Description = "Complete a \"Change Archetype\" mission",
                // filename: splitpersonality
                UnlockedFileNameHash = "0x201ec9d9",
                // filename: splitpersonality_disabled
                LockedFileNameHash = "0x9c04bebc",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-42
                Id = 0xf88c1342,
                Name = "42",
                Description = "Find the center of the galaxy",
                // filename: 42
                UnlockedFileNameHash = "0x1d76aa83",
                // filename: 42_disabled
                LockedFileNameHash = "0xff7481ee",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-manifestdestiny
                Id = 0x7d4eb7d3,
                Name = "Manifest Destiny",
                Description = "Find Earth",
                // filename: manifestdestiny
                UnlockedFileNameHash = "0xb0d6c0dc",
                // filename: manifestdestiny_disabled
                LockedFileNameHash = "0x4539bd2b",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-empirebuilder
                Id = 0xfbe4dc0d,
                Name = "Empire Builder",
                Description = "Maximize colonies on 10 planets",
                // filename: empirebuilder
                UnlockedFileNameHash = "0xabe3d20e",
                // filename: empirebuilder_disabled
                LockedFileNameHash = "0x22c8ec05",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-zookeeper
                Id = 0xcfc6c8ee,
                Name = "Zoo Keeper",
                Description = "Make 15 zoo planets",
                // filename: zookeeper
                UnlockedFileNameHash = "0x145620ed",
                // filename: zookeeper_disabled
                LockedFileNameHash = "0x228ea920",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-mb10trader
                Id = 0x39b4e19a,
                Name = "Trader Hero",
                Description = "Achieve Master Badge Level 10 as a Trader",
                // filename: traderhero
                UnlockedFileNameHash = "0x34a253f5",
                // filename: traderhero_disabled
                LockedFileNameHash = "0x852953c8",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-carelessparent
                Id = 0xfd534550,
                Name = "Careless Parent",
                Description = "Lose 5 planets",
                // filename: carelessparent
                UnlockedFileNameHash = "0x4fb88e75",
                // filename: carelessparent_disabled
                LockedFileNameHash = "0xdb22db48",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-alterego
                Id = 0x99bb05bb,
                Name = "Alter Ego",
                Description = "Play the Space stage as all 10 archetypes",
                // filename: alterego
                UnlockedFileNameHash = "0x9991ad5a",
                // filename: alterego_disabled
                LockedFileNameHash = "0x214c3881",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-archetypediplomat
                Id = 0x59bf4bc7,
                Name = "Diplomatic Passion",
                Description = "Play as a Diplomat",
                // filename: DiplomatPassion
                UnlockedFileNameHash = "0xf85034fe",
                // filename: DiplomatPassion_disabled
                LockedFileNameHash = "0x822c8015",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-archetypeknight
                Id = 0x5ae3a08a,
                Name = "Knight Passion",
                Description = "Play as a Knight",
                // filename: KnightPassion
                UnlockedFileNameHash = "0xe6455a1d",
                // filename: KnightPassion_disabled
                LockedFileNameHash = "0xf54c9630",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-mb10warrior
                Id = 0x33f68284,
                Name = "Warrior Hero",
                Description = "Achieve Master Badge Level 10 as a Warrior",
                // filename: warriorhero
                UnlockedFileNameHash = "0xa64548fd",
                // filename: warriorhero_disabled
                LockedFileNameHash = "0xe0b78850",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-mb10bard
                Id = 0xa9afd0df,
                Name = "Bard Hero",
                Description = "Achieve Master Badge Level 10 as a Bard",
                // filename: BardHero
                UnlockedFileNameHash = "0xd7cd4934",
                // filename: BardHero_disabled
                LockedFileNameHash = "0x2e377e23",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-bioeng
                Id = 0xc287732e,
                Name = "Bio-Engineer",
                Description = "Edit 25 creatures with the Creature Tweaker",
                // filename: bioengineer
                UnlockedFileNameHash = "0x58d6fd3e",
                // filename: bioengineer_disabled
                LockedFileNameHash = "0x9e8aa355",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-mb10knight
                Id = 0x7dca8049,
                Name = "Knight Hero",
                Description = "Achieve Master Badge Level 10 as a Knight",
                // filename: knighthero
                UnlockedFileNameHash = "0xf8357f06",
                // filename: knighthero_disabled
                LockedFileNameHash = "0x11c3bffd",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-superpilot
                Id = 0x256184b1,
                Name = "Super Pilot",
                Description = "Spend at least 40 hours piloting your spaceship",
                // filename: pilot
                UnlockedFileNameHash = "0x6dcfb1b9",
                // filename: pilot_disabled
                LockedFileNameHash = "0x7d36de5c",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-gunner
                Id = 0x2a7c313d,
                Name = "Gunner",
                Description = "Destroy at least 500 other space vessels",
                // filename: gunner
                UnlockedFileNameHash = "0xa34fd03c",
                // filename: gunner_disabled
                LockedFileNameHash = "0x1a6f084b",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-alterego2
                Id = 0xbc6a0553,
                Name = "Alter Ego's Alter Ego",
                Description = "Achieve Master Badge Level 10 as all 10 archetypes",
                // filename: alteregosalterego
                UnlockedFileNameHash = "0x1a14cc72",
                // filename: alteregosalterego_disabled
                LockedFileNameHash = "0xd158c899",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-mb10ecologist
                Id = 0x53f9ee27,
                Name = "Ecologist Hero",
                Description = "Achieve Master Badge Level 10 as an Ecologist",
                // filename: 0x3ED0ABB5
                UnlockedFileNameHash = "0x3ed0abb5",
                // filename: 0x4CDFD088
                LockedFileNameHash = "0x4cdfd088",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-archetypezealot
                Id = 0xcb7d6d3c,
                Name = "Zealot Passion",
                Description = "Play as a Zealot",
                // filename: ZealotPassion
                UnlockedFileNameHash = "0x66c464b3",
                // filename: ZealotPassion_disabled
                LockedFileNameHash = "0x3d34963e",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-mb10diplomat
                Id = 0xdc2042dc,
                Name = "Diplomatic Hero",
                Description = "Achieve Master Badge Level 10 as a Diplomat",
                // filename: diplomathero
                UnlockedFileNameHash = "0x110deaef",
                // filename: diplomathero_disabled
                LockedFileNameHash = "0xfe9f866a",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-thief
                Id = 0x7c1a0684,
                Name = "Thief",
                Description = "Steal 50 crates of spice in the Space stage",
                // filename: thief
                UnlockedFileNameHash = "0x46807a4f",
                // filename: thief_disabled
                LockedFileNameHash = "0xac3f450a",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-identitycrisis
                Id = 0x4106aa7d,
                Name = "Identity Crisis",
                Description = "Forge an alliance between two space-faring races of your own creation",
                // filename: identitycrisis
                UnlockedFileNameHash = "0xaa6f4d24",
                // filename: identitycrisis_disabled
                LockedFileNameHash = "0x91706533",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-engineer
                Id = 0x313be103,
                Name = "Civil Engineer",
                Description = "Promote 20 alien Tribes to Civilizations",
                // filename: CivilEngineer
                UnlockedFileNameHash = "0xb01b6a61",
                // filename: CivilEngineer_disabled
                LockedFileNameHash = "0xe6fb2264",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-archetypewarrior
                Id = 0x2b54d82d,
                Name = "Warrior Passion",
                Description = "Play as a Warrior",
                // filename: WarriorPassion
                UnlockedFileNameHash = "0xd3cf48b0",
                // filename: WarriorPassion_disabled
                LockedFileNameHash = "0x023c40cf",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-questmaster
                Id = 0xf5d3b6b4,
                Name = "Quest Master",
                Description = "Complete 150 missions in the Space stage",
                // filename: questmaster
                UnlockedFileNameHash = "0xb469fe27",
                // filename: questmaster_disabled
                LockedFileNameHash = "0x060a3f42",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-archetypeshaman
                Id = 0x7afcb267,
                Name = "Shaman Passion",
                Description = "Play as a Shaman",
                // filename: shamanPassion
                UnlockedFileNameHash = "0x6656b316",
                // filename: shamanPassion_disabled
                LockedFileNameHash = "0x871f986d",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-archetypescientist
                Id = 0x3e06ba31,
                Name = "Scientist Passion",
                Description = "Play as a Scientist",
                // filename: scientistPassion
                UnlockedFileNameHash = "0x042f148c",
                // filename: scientistPassion_disabled
                LockedFileNameHash = "0x2d0cc37b",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-mb10shaman
                Id = 0x278f32d4,
                Name = "Shaman Hero",
                Description = "Achieve Master Badge Level 10 as a Shaman",
                // filename: shamanhero
                UnlockedFileNameHash = "0x6cffa187",
                // filename: shamanhero_disabled
                LockedFileNameHash = "0x7e56aee2",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-palmgreaser
                Id = 0x77893b39,
                Name = "Palm Greaser",
                Description = "Pay 50 bribes in the Space stage",
                // filename: palmgreaser
                UnlockedFileNameHash = "0x59333a86",
                // filename: palmgreaser_disabled
                LockedFileNameHash = "0xab3cba7d",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: spg-archetypeecologist
                Id = 0xda2fec0e,
                Name = "Ecologist Passion",
                Description = "Play as an Ecologist",
                // filename: EcologistPassion
                UnlockedFileNameHash = "0xf171f959",
                // filename: EcologistPassion_disabled
                LockedFileNameHash = "0x8d9ad53c",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-CaptainAcademy
                Id = 0x00b5e59f,
                Name = "Captain Academy",
                Description = "Raise 10 Captains to Rank 10.",
                // filename: 0xDC7DD1C5
                UnlockedFileNameHash = "0xdc7dd1c5",
                // filename: 0x49E3D878
                LockedFileNameHash = "0x49e3d878",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-IndustriousTrader
                Id = 0x16913702,
                Name = "Industrious Trader",
                Description = "Unlock all four Trader parts on a Captain.",
                // filename: 0xA81B454B
                UnlockedFileNameHash = "0xa81b454b",
                // filename: 0x99D54836
                LockedFileNameHash = "0x99d54836",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-PublishedAuthor
                Id = 0x1881b6ec,
                Name = "Published Author",
                Description = "Publish an Adventure.",
                // filename: PublishedAuthor
                UnlockedFileNameHash = "0xb2b84f48",
                // filename: PublishedAuthor_disabled
                LockedFileNameHash = "0x11aed0c7",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-IntelligentScientist
                Id = 0x1c342ee2,
                Name = "Intelligent Scientist",
                Description = "Unlock all four Scientist parts on a Captain.",
                // filename: 0x3ADFCF3F
                UnlockedFileNameHash = "0x3adfcf3f",
                // filename: 0x5156255A
                LockedFileNameHash = "0x5156255a",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-DemolitionExpert
                Id = 0x2f8fab8f,
                Name = "Demolition Expert",
                Description = "Destroy 1000 Buildings while playing Adventures in the Space Stage.",
                // filename: DemolitionExpert
                UnlockedFileNameHash = "0x460a98f3",
                // filename: DemolitionExpert_disabled
                LockedFileNameHash = "0x50ad467e",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-Mercenary
                Id = 0x34c7c0af,
                Name = "Mercenary",
                Description = "Kill 1000 Creatures while playing Adventures in the Space Stage.",
                // filename: Mercenary
                UnlockedFileNameHash = "0xc3fc2d93",
                // filename: Mercenary_disabled
                LockedFileNameHash = "0x48d173de",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-HappyTimes
                Id = 0x3b23cfa9,
                Name = "Happy Times",
                Description = "Create 30 Adventures with all peaceful actors.",
                // filename: HappyTimes
                UnlockedFileNameHash = "0x1d5ee23d",
                // filename: HappyTimes_disabled
                LockedFileNameHash = "0xccb52890",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-GearedUp
                Id = 0x42df8d08,
                Name = "Geared Up",
                Description = "Outfit 50 Creatures with Adventure parts.",
                // filename: GearedUp
                UnlockedFileNameHash = "0x9d7259bc",
                // filename: GearedUp_disabled
                LockedFileNameHash = "0x9c578ccb",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-Auto-Wrecker
                Id = 0x452ef89c,
                Name = "Auto-Wrecker",
                Description = "Destroy 1000 Vehicles while playing Adventures in the Space Stage.",
                // filename: AutoWrecker
                UnlockedFileNameHash = "0xb3c23a6f",
                // filename: AutoWrecker_disabled
                LockedFileNameHash = "0x8766fcea",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-Aeschylus
                Id = 0x48cdfa34,
                Name = "Aeschylus",
                Description = "Create 10 Adventures.",
                // filename: Aeschylus
                UnlockedFileNameHash = "0xdcb3a220",
                // filename: Aeschylus_disabled
                LockedFileNameHash = "0xb637101f",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-SocialButterfly
                Id = 0x495f48dd,
                Name = "Social Butterfly",
                Description = "Befriend 1000 Creatures while playing Adventures in the Space Stage.",
                // filename: SocialButterfly
                UnlockedFileNameHash = "0xaec50631",
                // filename: SocialButterfly_disabled
                LockedFileNameHash = "0xbc4cda54",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-Storyteller
                Id = 0x4a384f78,
                Name = "Storyteller",
                Description = "Publish 50 Adventures.",
                // filename: Storyteller
                UnlockedFileNameHash = "0x335a25b4",
                // filename: Storyteller_disabled
                LockedFileNameHash = "0x3e4ccba3",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-Adventurer
                Id = 0x51684fd3,
                Name = "Adventurer",
                Description = "Complete an Adventure in the Space Stage.",
                // filename: Adventurer
                UnlockedFileNameHash = "0xc18cb027",
                // filename: Adventurer_disabled
                LockedFileNameHash = "0x93793542",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-Emperor
                Id = 0x5191a2e3,
                Name = "Emperor",
                Description = "Place 200 Empire Creatures in your Adventures.",
                // filename: Emperor
                UnlockedFileNameHash = "0xeca28cd7",
                // filename: Emperor_disabled
                LockedFileNameHash = "0x0b7a0bd2",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-DeliriousDesigner
                Id = 0x5c324dc2,
                Name = "Delirious Designer",
                Description = "Create 50 Adventures using only your own creations.",
                // filename: 0x8F47CDF7
                UnlockedFileNameHash = "0x8f47cdf7",
                // filename: 0xA26B2A32
                LockedFileNameHash = "0xa26b2a32",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-Mechanic
                Id = 0x60aec253,
                Name = "Mechanic",
                Description = "Place 500 Vehicles into your Adventures.",
                // filename: Mechanic
                UnlockedFileNameHash = "0x0e0f3d77",
                // filename: Mechanic_disabled
                LockedFileNameHash = "0x695a00b2",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-FaithfulZealot
                Id = 0x61104b3d,
                Name = "Faithful Zealot",
                Description = "Unlock all four Zealot parts on a Captain.",
                // filename: 0xC0FFD6F0
                UnlockedFileNameHash = "0xc0ffd6f0",
                // filename: 0xF0EBCE0F
                LockedFileNameHash = "0xf0ebce0f",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-Scientist
                Id = 0x62d8b529,
                Name = "Scientist",
                Description = "Place 200 Non-Accessorized Creatures into your Adventures.",
                // filename: scientist
                UnlockedFileNameHash = "0x519c1e5d",
                // filename: scientist_disabled
                LockedFileNameHash = "0x612d4370",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-MyPrecious
                Id = 0x78191fc1,
                Name = "My Precious",
                Description = "Collect 3000 Objects while playing Adventures in the Space Stage.",
                // filename: MyPrecious
                UnlockedFileNameHash = "0x83dec6bd",
                // filename: MyPrecious_disabled
                LockedFileNameHash = "0xc77b0e10",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-UniversalEcologist
                Id = 0x79771a4d,
                Name = "Universal Ecologist",
                Description = "Unlock all four Ecologist parts on a Captain.",
                // filename: 0x2DC31F4A
                UnlockedFileNameHash = "0x2dc31f4a",
                // filename: 0x755D1B51
                LockedFileNameHash = "0x755d1b51",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-FierceWarrior
                Id = 0x819d7bab,
                Name = "Fierce Warrior",
                Description = "Unlock all four Warrior Parts on a Captain.",
                // filename: 0x8ABED973
                UnlockedFileNameHash = "0x8abed973",
                // filename: 0x5C267FFE
                LockedFileNameHash = "0x5c267ffe",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-Architect
                Id = 0x836a79ce,
                Name = "Architect",
                Description = "Place 500 Buildings into your Adventures.",
                // filename: 0x5847ABC0
                UnlockedFileNameHash = "0x5847abc0",
                // filename: 0x429CB3FF
                LockedFileNameHash = "0x429cb3ff",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-LargeEnsemble
                Id = 0x88b15773,
                Name = "Large Ensemble",
                Description = "Create 30 Adventures with over 100 actors in each.",
                // filename: LargeEnsemble
                UnlockedFileNameHash = "0x8112393f",
                // filename: LargeEnsemble_disabled
                LockedFileNameHash = "0x92b0c35a",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-CaptainAcademy
                Id = 0x00b5e59f,
                Name = "My First Captain",
                Description = "Raise a Captain to Rank 10.",
                // filename: 0xDC7DD1C5
                UnlockedFileNameHash = "0xdc7dd1c5",
                // filename: 0x49E3D878
                LockedFileNameHash = "0x49e3d878",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-GrandAdventurer
                Id = 0x9c119e6b,
                Name = "Grand Adventurer",
                Description = "Complete 100 Adventures inside the Space Stage.",
                // filename: GrandAdventurer
                UnlockedFileNameHash = "0x6fd71f5f",
                // filename: GrandAdventurer_disabled
                LockedFileNameHash = "0xe4ca0eba",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-JustDiplomat
                Id = 0xa7a3dd49,
                Name = "Just Diplomat",
                Description = "Unlock all four Diplomat parts on a Captain.",
                // filename: 0x72437DCD
                UnlockedFileNameHash = "0x72437dcd",
                // filename: 0xA98895C0
                LockedFileNameHash = "0xa98895c0",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-CostumePiece
                Id = 0xa9d241a5,
                Name = "Costume Piece",
                Description = "Create 50 Adventures with Tribal, Civilized and Empire Creatures.",
                // filename: CostumePiece
                UnlockedFileNameHash = "0x6f472289",
                // filename: CostumePiece_disabled
                LockedFileNameHash = "0xe19d044c",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-Moliere
                Id = 0xabc8af1a,
                Name = "Moliere",
                Description = "Create 50 Adventures.",
                // filename: Moliere
                UnlockedFileNameHash = "0x30269f86",
                // filename: Moliere_disabled
                LockedFileNameHash = "0xa805997d",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-SoCivilized
                Id = 0xaeda120e,
                Name = "So Civilized",
                Description = "Place 200 Civilized Creatures in your Adventures.",
                // filename: SoCivilized
                UnlockedFileNameHash = "0x5f5ed79a",
                // filename: SoCivilized_disabled
                LockedFileNameHash = "0xe85725c1",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-RedShirt
                Id = 0xb57b9d12,
                Name = "Red Shirt",
                Description = "Lose 100 Crew Members while playing Adventures in the Space Stage.",
                // filename: RedShirt
                UnlockedFileNameHash = "0x4f6792e6",
                // filename: RedShirt_disabled
                LockedFileNameHash = "0xfdb93d1d",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-CaptainAcademy
                Id = 0x00b5e59f,
                Name = "Wise Shaman",
                Description = "Unlock all four Shaman parts on a Captain.",
                // filename: 0xDC7DD1C5
                UnlockedFileNameHash = "0xdc7dd1c5",
                // filename: 0x49E3D878
                LockedFileNameHash = "0x49e3d878",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-DragonSlayer
                Id = 0xcba1a092,
                Name = "Dragon Slayer",
                Description = "Kill 1000 Epics while playing Adventures in the Space Stage.",
                // filename: DragonSlayer
                UnlockedFileNameHash = "0xf1f8f26e",
                // filename: DragonSlayer_disabled
                LockedFileNameHash = "0xac5f3f25",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-Dabbler
                Id = 0xcccf91a1,
                Name = "Dabbler",
                Description = "Download 500 Adventures.",
                // filename: Dabbler
                UnlockedFileNameHash = "0x92ccb60d",
                // filename: Dabbler_disabled
                LockedFileNameHash = "0x5fef2600",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-StillLife
                Id = 0xe55af32f,
                Name = "Still Life",
                Description = "Create 30 Adventures that do not contain Creatures.",
                // filename: StillLife
                UnlockedFileNameHash = "0xc26c79f3",
                // filename: StillLife_disabled
                LockedFileNameHash = "0x1255d97e",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-Terraformer
                Id = 0xe5e6d992,
                Name = "Terraformaniac",
                Description = "Create 50 Adventures using Planets you terraformed.",
                // filename: Terraformer
                UnlockedFileNameHash = "0xca5c5586",
                // filename: Terraformer_disabled
                LockedFileNameHash = "0x81815b7d",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-Shakespeare
                Id = 0xec310ad9,
                Name = "Shakespeare",
                Description = "Create 100 Adventures.",
                // filename: Shakespeare
                UnlockedFileNameHash = "0x49729585",
                // filename: Shakespeare_disabled
                LockedFileNameHash = "0xee64d538",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-GoingTribal
                Id = 0xeec38869,
                Name = "Going Tribal",
                Description = "Place 200 Tribal Creatures in your Adventures.",
                // filename: GoingTribal
                UnlockedFileNameHash = "0x3f0d722d",
                // filename: GoingTribal_disabled
                LockedFileNameHash = "0x754fec60",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-Consumer
                Id = 0xf332be69,
                Name = "Consumer",
                Description = "Successfully complete 200 Adventures made by other Creators.",
                // filename: Consumer
                UnlockedFileNameHash = "0x4a6d0c8d",
                // filename: Consumer_disabled
                LockedFileNameHash = "0xc2014180",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-Bestseller
                Id = 0xf6b358fa,
                Name = "Bestseller",
                Description = "Create a Maxis featured Adventure.",
                // filename: Bestseller
                UnlockedFileNameHash = "0xb7fc00f6",
                // filename: Bestseller_disabled
                LockedFileNameHash = "0xef8b198d",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-JoyfulBard
                Id = 0xf9ea822f,
                Name = "Joyful Bard",
                Description = "Unlock all four Bard parts on a Captain.",
                // filename: 0xD9D677EC
                UnlockedFileNameHash = "0xd9d677ec",
                // filename: 0x39683F1B
                LockedFileNameHash = "0x39683f1b",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-MyHero
                Id = 0xfcace0d7,
                Name = "My Hero",
                Description = "Protect 500 Creatures while playing Adventures in the Space Stage.",
                // filename: MyHero
                UnlockedFileNameHash = "0x5576430b",
                // filename: MyHero_disabled
                LockedFileNameHash = "0xdf9259f6",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-Granddaddy
                Id = 0xfd31e08d,
                Name = "Granddaddy",
                Description = "Have 10 other creators edit one of your Adventures.",
                // filename: Granddaddy
                UnlockedFileNameHash = "0xad53c2e9",
                // filename: Granddaddy_disabled
                LockedFileNameHash = "0x678fb4ec",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: ADV-MajorContributor
                Id = 0xfe59d503,
                Name = "Major Contributor",
                Description = "Give 500 Gifts while playing Adventures in the Space Stage.",
                // filename: MajorContributor
                UnlockedFileNameHash = "0x798b90ef",
                // filename: MajorContributor_disabled
                LockedFileNameHash = "0x1a64186a",
                Secret = false
            },
            new SporeAchievement()
            {
                // filename: alg-badbaby
                Id = 0x7136827f,
                Name = "Bad Baby!",
                Description = "Have one of your assets banned",
                // filename: badbaby
                UnlockedFileNameHash = "0xea5bcb5a",
                // filename: badbaby_disabled
                LockedFileNameHash = "0xd7a6b281",
                Secret = true
            },
            new SporeAchievement()
            {
                // filename: alg-cheater
                Id = 0xa3434a32,
                Name = "Pathological Cheater",
                Description = "Use a cheat more than 50 times",
                // filename: PathologicalCheater
                UnlockedFileNameHash = "0x012d52b0",
                // filename: PathologicalCheater_disabled
                LockedFileNameHash = "0x69ffbecf",
                Secret = true
            },
            new SporeAchievement()
            {
                // filename: alg-purpleheart
                Id = 0xd456d958,
                Name = "Can't Win For Losing",
                Description = "Die at least once in every stage of Spore",
                // filename: purpleheart
                UnlockedFileNameHash = "0x8996a321",
                // filename: purpleheart_disabled
                LockedFileNameHash = "0xe8cfb824",
                Secret = true
            },
            new SporeAchievement()
            {
                // filename: crg-cerberus
                Id = 0x58ab9d73,
                Name = "Cerberus",
                Description = "Evolve a creature with three heads",
                // filename: cerberus
                UnlockedFileNameHash = "0xdb9d410c",
                // filename: cerberus_disabled
                LockedFileNameHash = "0x500100fb",
                Secret = true
            },
            new SporeAchievement()
            {
                // filename: crg-generalcuster
                Id = 0xfbf10c5e,
                Name = "General Custer",
                Description = "Lead 30 posse members to their death",
                // filename: generalcuster
                UnlockedFileNameHash = "0xe5a6c367",
                // filename: generalcuster_disabled
                LockedFileNameHash = "0x1cec3082",
                Secret = true
            },
            new SporeAchievement()
            {
                // filename: crg-slugger
                Id = 0x6e36e67f,
                Name = "Slugger",
                Description = "Finish Creature stage without legs",
                // filename: slugger
                UnlockedFileNameHash = "0x61aa6482",
                // filename: slugger_disabled
                LockedFileNameHash = "0x6ae96dc9",
                Secret = true
            },
            new SporeAchievement()
            {
                // filename: trg-medic
                Id = 0x90ec8983,
                Name = "Medic",
                Description = "Heal one of your tribe members back to full health 5x in a single game",
                // filename: medic
                UnlockedFileNameHash = "0xbb4d62d9",
                // filename: medic_disabled
                LockedFileNameHash = "0xe3fff9bc",
                Secret = true
            },
            new SporeAchievement()
            {
                // filename: cvg-conclusion
                Id = 0x1696afcf,
                Name = "Conclusion",
                Description = "Finish the Civilization stage by launching ICBMS and destroying all other cities",
                // filename: conclusion
                UnlockedFileNameHash = "0xadb46f84",
                // filename: conclusion_disabled
                LockedFileNameHash = "0x629734d3",
                Secret = true
            },
            new SporeAchievement()
            {
                // filename: cvg-fearofflying
                Id = 0x9d649052,
                Name = "Fear of Flying",
                Description = "Finish the Civilization stage without ever buying an aircraft",
                // filename: fearofflying
                UnlockedFileNameHash = "0x18a3efe1",
                // filename: fearofflying_disabled
                LockedFileNameHash = "0x90b1dae4",
                Secret = true
            },
            new SporeAchievement()
            {
                // filename: spg-humanity
                Id = 0xc280a311,
                Name = "Oh the Humanity!",
                Description = "Destroy Earth",
                // filename: OhTheHumanity
                UnlockedFileNameHash = "0x039a1b5c",
                // filename: OhTheHumanity_disabled
                LockedFileNameHash = "0x4c88a4ab",
                Secret = true
            },
            new SporeAchievement()
            {
                // filename: ADV-ChatterBox
                Id = 0x04cdfa83,
                Name = "Chatter Box",
                Description = "Comment on 200 Adventures.",
                // filename: ChatterBox
                UnlockedFileNameHash = "0xf836d55f",
                // filename: ChatterBox_disabled
                LockedFileNameHash = "0x5023d0ba",
                Secret = true
            },
            new SporeAchievement()
            {
                // filename: ADV-EpicFail
                Id = 0x23346746,
                Name = "Epic Fail",
                Description = "Fail 50 Adventures made by other Creators.",
                // filename: EpicFail
                UnlockedFileNameHash = "0x3c2e290a",
                // filename: EpicFail_disabled
                LockedFileNameHash = "0xf9047b11",
                Secret = true
            },
            new SporeAchievement()
            {
                // filename: ADV-MaxisSuperFan
                Id = 0x280d4a31,
                Name = "Maxis Super Fan",
                Description = "Create 25 Adventures using only Maxis made creations.",
                // filename: MaxisSuperFan
                UnlockedFileNameHash = "0xc7bef9bd",
                // filename: MaxisSuperFan_disabled
                LockedFileNameHash = "0xb38b7710",
                Secret = true
            },
            new SporeAchievement()
            {
                // filename: ADV-TopSeller
                Id = 0x3f4e3597,
                Name = "Top Seller",
                Description = "Have 1000 of your creations used in other creator's Adventures.",
                // filename: TopSeller
                UnlockedFileNameHash = "0x718c772b",
                // filename: TopSeller_disabled
                LockedFileNameHash = "0xf6b263d6",
                Secret = true
            },
            new SporeAchievement()
            {
                // filename: ADV-TheCritic
                Id = 0xa29f7372,
                Name = "The Critic",
                Description = "Rate 200 Adventures.",
                // filename: TheCritic
                UnlockedFileNameHash = "0x8a020b4e",
                // filename: TheCritic_disabled
                LockedFileNameHash = "0xd5c60545",
                Secret = true
            },
            new SporeAchievement()
            {
                // filename: ADV-MadSkills
                Id = 0xc5013ecf,
                Name = "Mad Skills",
                Description = "Earn top ranks on 50 Leaderboards.",
                // filename: MadSkills
                UnlockedFileNameHash = "0xdd798623",
                // filename: MadSkills_disabled
                LockedFileNameHash = "0x9e20dace",
                Secret = true
            }
        };
    }
}

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace EcommerceSite1.Models {
    public class Game {
        //public static readonly ImmutableList<Game> _defaultGamesList = new Game[] {
        //        new Game() {
        //           // ID = 0,
        //            ImageURL="/Resources/GameImages/DarkSouls.jpg",
        //            Title="Dark Souls",
        //            Link="/Catalog/DarkSouls",
        //            Price=19.99m,
        //            LongDescription = "Dark Souls is the new action role-playing game from the developers who brought you Demon’s Souls, FromSoftware. Dark Souls will have many familiar features: A dark fantasy universe, tense dungeon crawling, fearsome enemy encounters and unique online interactions. Dark Souls is a spiritual successor to Demon’s, not a sequel. Prepare for a new, despair-inducing world, with a vast, fully-explorable horizon and vertically-oriented landforms. Prepare for a new, mysterious story, centered around the the world of Lodran, but most of all, prepare to die. You will face countless murderous traps, countless darkly grotesque mobs and several gargantuan, supremely powerful demons and dragons bosses. You must learn from death to persist through this unforgiving world. And you aren’t alone. Dark Souls allows the spirits of other players to show up in your world, so you can learn from their deaths and they can learn from yours. You can also summon players into your world to co-op adventure, or invade other's worlds to PVP battle. New to Dark Souls are Bonfires, which serve as check points as you fight your way through this epic adventure. While rested at Bonfires, your health and magic replenish but at a cost, all mobs respawn. Beware: There is no place in Dark Souls that is truly safe. With days of game play and an even more punishing difficulty level, Dark Souls will be the most deeply challenging game you play this year. Can you live through a million deaths and earn your legacy?",
        //            ShortDescription = "\"Dark Souls difficulty is like crushing cinder blocks with your face: there's a technique to it, but you won't figure it out until after you've knocked out all your teeth.\""
        //        },
        //        new Game() {
        //            ImageURL="/Resources/GameImages/SuperMeatBoy.jpg",
        //            Title="Super Meat Boy",
        //            Link="/Catalog/SuperMeatBoy",
        //            Price=14.99m,
        //            LongDescription = "Our meaty hero will leap from walls, over seas of buzz saws, through crumbling caves and pools of old needles. Sacrificing his own well being to save his damsel in distress. Super Meat Boy brings the old school difficulty of classic NES titles like Mega Man 2, Ghost and Goblins and Super Mario Bros. 2 (The Japanese one) and stream lines them down to the essential no BS straight forward twitch reflex platforming. Ramping up in difficulty from hard to soul crushing SMB will drag Meat boy though haunted hospitals, salt factories and even hell itself. And if 300+ single player levels weren't enough SMB also throws in epic boss fights, a level editor and tons of unlock able secrets, warp zones and hidden characters.",
        //            ShortDescription = "\"A tough as nails platformer where you play as an animated cube of meat who's trying to save his girlfriend (who happens to be made of bandages) from an evil fetus in a jar wearing a tux.\""
        //        },
        //        new Game() {
        //           // ID = 2,
        //            ImageURL="/Resources/GameImages/Fez.jpg",
        //            Title="Fez",
        //            Link="/Catalog/Fez",
        //            Price=9.99m,
        //            LongDescription = "Gomez is a 2D creature living in a 2D world. Or is he? When the existence of a mysterious 3rd dimension is revealed to him, Gomez is sent out on a journey that will take him to the very end of time and space. Use your ability to navigate 3D structures from 4 distinct classic 2D perspectives. Explore a serene and beautiful open-ended world full of secrets, puzzles and hidden treasures. Unearth the mysteries of the past and discover the truth about reality and perception. Change your perspective and look at the world in a different way.",
        //            ShortDescription = "\"The game's unique artwork, its perspective-shift mechanic, its nostalgia for the 16-bit years and its bewitchingly strange setting all exist in total harmony and make a single, deliberate statement.\""
        //        }
        //    }.ToImmutableList();
        //public static readonly ImmutableDictionary<string, Game> _defaultGames = _defaultGamesList.ToImmutableDictionary<Game, string>((x) => x.Title.Replace(" ", String.Empty));
        
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "An ImageURL is required.")]
        public string ImageURL { get; set; }
        
        private decimal price;
        [Required(ErrorMessage = "A Price is required.")]
        [Index]
        public decimal Price {
            get {
                return price;
            }
            set {
                if(value >= 0)
                    price = Math.Round(value, 2);
            }
        }

        private string title;
        [Key, Column(Order = 1)]
        [Index(IsUnique = true)]
        [MaxLength(255)]
        [Required(ErrorMessage = "Title is required.")]
        public string Title {
            get {
                return title;
            }
            set {
                title = value;
                Title_Spaceless = value;
            }
        }
        
        private string title_spaceless;
        [Index(IsUnique = true)]
        [MaxLength(255)]
        public string Title_Spaceless {
            get {
                if (title_spaceless == null) {
                    Title_Spaceless = Title;
                }
                return title_spaceless;
            }
            private set {
                title_spaceless = value?.Replace(" ", String.Empty);
                Link = Title_Spaceless;
            }
        }
        private string link;
        public string Link {
            get {
                return link;
            }
            private set {
                if (value != null && link == null)
                    link = "/Game/Game/" + value;
            }
        }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "A ShortDescription is required.")]
        public string ShortDescription { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "A LongDescription is required.")]
        public string LongDescription { get; set; }
    }

    public class GameDB: DbContext {
        public GameDB() {
            Database.SetInitializer<GameDB>(new GameDBInit<GameDB>());
        }
        public GameDB(string connection_name) : base(connection_name) {
            Database.SetInitializer<GameDB>(new GameDBInit<GameDB>());
        }
        public DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }

    public class GameDBInit<T> : DropCreateDatabaseAlways<GameDB> where T: new() {
        protected override void Seed(GameDB context) {
            context.Games.Add(
                new Game() {
                    // ID = 0,
                    ImageURL = "/Resources/GameImages/DarkSouls.jpg",
                    Title = "Dark Souls",
                    //Link="/Catalog/DarkSouls",
                    Price = 19.99m,
                    LongDescription = "Dark Souls is the new action role-playing game from the developers who brought you Demon’s Souls, FromSoftware. Dark Souls will have many familiar features: A dark fantasy universe, tense dungeon crawling, fearsome enemy encounters and unique online interactions. Dark Souls is a spiritual successor to Demon’s, not a sequel. Prepare for a new, despair-inducing world, with a vast, fully-explorable horizon and vertically-oriented landforms. Prepare for a new, mysterious story, centered around the the world of Lodran, but most of all, prepare to die. You will face countless murderous traps, countless darkly grotesque mobs and several gargantuan, supremely powerful demons and dragons bosses. You must learn from death to persist through this unforgiving world. And you aren’t alone. Dark Souls allows the spirits of other players to show up in your world, so you can learn from their deaths and they can learn from yours. You can also summon players into your world to co-op adventure, or invade other's worlds to PVP battle. New to Dark Souls are Bonfires, which serve as check points as you fight your way through this epic adventure. While rested at Bonfires, your health and magic replenish but at a cost, all mobs respawn. Beware: There is no place in Dark Souls that is truly safe. With days of game play and an even more punishing difficulty level, Dark Souls will be the most deeply challenging game you play this year. Can you live through a million deaths and earn your legacy?",
                    ShortDescription = "\"Dark Souls difficulty is like crushing cinder blocks with your face: there's a technique to it, but you won't figure it out until after you've knocked out all your teeth.\""
                }
            );
            context.Games.Add(
                new Game() {
                    // ID = 0,
                    ImageURL = "http://cdn.akamai.steamstatic.com/steam/apps/236430/ss_9da48e604ad0d59add1a2fdec0140c1e052247cf.600x338.jpg?t=1447357725",
                    Title = "Dark Souls II",
                    //Link="/Catalog/DarkSouls",
                    Price = 39.99m,
                    LongDescription = "Developed by FROM SOFTWARE, DARK SOULS™ II is the highly anticipated sequel to the gruelling 2011 breakout hit Dark Souls. The unique old-school action RPG experience captivated imaginations of gamers worldwide with incredible challenge and intense emotional reward. DARK SOULS™ II brings the franchise’s renowned obscurity & gripping gameplay innovations to both single and multiplayer experiences.\"",
                    ShortDescription = "\"Join the dark journey and experience overwhelming enemy encounters, diabolical hazards, and the unrelenting challenge that only FROM SOFTWARE can deliver.\""
                }
            );
            context.Games.Add(
                new Game() {
                    ImageURL = "/Resources/GameImages/SuperMeatBoy.jpg",
                    Title = "Super Meat Boy",
                    // Link="/Catalog/SuperMeatBoy",
                    Price = 14.99m,
                    LongDescription = "Our meaty hero will leap from walls, over seas of buzz saws, through crumbling caves and pools of old needles. Sacrificing his own well being to save his damsel in distress. Super Meat Boy brings the old school difficulty of classic NES titles like Mega Man 2, Ghost and Goblins and Super Mario Bros. 2 (The Japanese one) and stream lines them down to the essential no BS straight forward twitch reflex platforming. Ramping up in difficulty from hard to soul crushing SMB will drag Meat boy though haunted hospitals, salt factories and even hell itself. And if 300+ single player levels weren't enough SMB also throws in epic boss fights, a level editor and tons of unlock able secrets, warp zones and hidden characters.",
                    ShortDescription = "\"A tough as nails platformer where you play as an animated cube of meat who's trying to save his girlfriend (who happens to be made of bandages) from an evil fetus in a jar wearing a tux.\""
                }
            );
            context.Games.Add(
                new Game() {
                    // ID = 2,
                    ImageURL = "/Resources/GameImages/Fez.jpg",
                    Title = "Fez",
                    //Link="/Catalog/Fez",
                    Price = 9.99m,
                    LongDescription = "Gomez is a 2D creature living in a 2D world. Or is he? When the existence of a mysterious 3rd dimension is revealed to him, Gomez is sent out on a journey that will take him to the very end of time and space. Use your ability to navigate 3D structures from 4 distinct classic 2D perspectives. Explore a serene and beautiful open-ended world full of secrets, puzzles and hidden treasures. Unearth the mysteries of the past and discover the truth about reality and perception. Change your perspective and look at the world in a different way.",
                    ShortDescription = "\"The game's unique artwork, its perspective-shift mechanic, its nostalgia for the 16-bit years and its bewitchingly strange setting all exist in total harmony and make a single, deliberate statement.\""
                }
            );
            context.Games.Add(
                new Game() {
                    // ID = 2,
                    ImageURL = "https://cdn.wccftech.com/wp-content/uploads/2017/06/Super-Mario-Odyssey-gameplay.jpg",
                    Title = "Super Mario Odyssey",
                    //Link="/Catalog/Fez",
                    Price = 59.99m,
                    LongDescription = "\"Thanks to heroic, hat-shaped Cappy, Mario's got new moves that'll make you rethink his traditional run-and-jump gameplay—like cap jump, cap throw, and capture. Use captured cohorts such as enemies, objects, and animals to progress through the game and uncover loads of hidden collectibles. And if you feel like playing with a friend, just pass them a Joy-Con™ controller! Player 1 controls Mario while Player 2 controls Cappy. This sandbox-style 3D Mario adventure—the first since 1996's beloved Super Mario 64™ and 2002's Nintendo GameCube™ classic Super Mario Sunshine™—is packed with secrets and surprises, plus exciting new kingdoms to explore.\"",
                    ShortDescription = "\"Use amazing new abilities—like the power to capture and control objects, animals, and enemies—to collect Power Moons so you can power up the Odyssey airship and save Princess Peach from Bowser's wedding plans!\""
                }
            );

            base.Seed(context);
        }
    }
}
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
        //            ID = 0,
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
        //            ID = 2,
        //            ImageURL="/Resources/GameImages/Fez.jpg",
        //            Title="Fez",
        //            Link="/Catalog/Fez",
        //            Price=9.99m,
        //            LongDescription = "Gomez is a 2D creature living in a 2D world. Or is he? When the existence of a mysterious 3rd dimension is revealed to him, Gomez is sent out on a journey that will take him to the very end of time and space. Use your ability to navigate 3D structures from 4 distinct classic 2D perspectives. Explore a serene and beautiful open-ended world full of secrets, puzzles and hidden treasures. Unearth the mysteries of the past and discover the truth about reality and perception. Change your perspective and look at the world in a different way.",
        //            ShortDescription = "\"The game's unique artwork, its perspective-shift mechanic, its nostalgia for the 16-bit years and its bewitchingly strange setting all exist in total harmony and make a single, deliberate statement.\""
        //        }
        //    }.ToImmutableList();
        //public static readonly ImmutableDictionary<string, Game> _defaultGames = _defaultGamesList.ToImmutableDictionary<Game, string>((x) => x.Title.Replace(" ", String.Empty));

        private const short shortDescriptionCharLength = 144;
        
        [Required(ErrorMessage = "value is required.")]
        public string ImageURL { get; set; }
        
        private decimal price;
        [Required(ErrorMessage = "value is required.")]
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
        [Key]
        [Index(IsUnique = true)]
        [MaxLength(255)]
        public string Title_Spaceless {
            get {
                if(title_spaceless == null) {
                    Title_Spaceless = Title;
                }
                return title_spaceless;
            }
            set {
                title_spaceless = value?.Replace(" ", String.Empty);
                Link = Title_Spaceless;
            }
        }
        private string link;
        public string Link {
            get {
                return link;
            }
            set {
                if(value != null)
                    link = "Catalog/" + value;
            }
        }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "value is required.")]
        public string ShortDescription { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "value is required.")]
        public string LongDescription { get; set; }
    }

    public class GameDB: DbContext {
        public DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
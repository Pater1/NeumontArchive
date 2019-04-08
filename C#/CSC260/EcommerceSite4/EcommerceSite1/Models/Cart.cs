using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EcommerceSite1.Models {
    public class Cart {
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int GameId { get; set; }
        public string UserId { get; set; }

        public int Count { get; set; }
    }

    public class CartDB : DbContext {
        private UsersDB users;
        private GameDB games;

        public void Purge(ApplicationUser forUser) {
            Cart[] cs = Carts.Where(x => forUser.Id == x.UserId).ToArray();
            foreach(Cart c in cs) {
                Carts.Remove(c);
            }
            SaveChanges();
        }

        public CartDB() {
            users = new UsersDB();
            games = new GameDB();
        }

        public CartDB(string connection_name) : base(connection_name) {
            users = new UsersDB();
            games = new GameDB();
        }
        public DbSet<Cart> Carts { get; set; }

        public void Add(Game game, ApplicationUser forUser, int number = 1) {
            Cart c = Carts.Where(x => x.GameId == game.Id && forUser.Id == x.UserId).FirstOrDefault();
            if(c == null) {
                c = new Cart();
                c.UserId = forUser.Id;
                c.GameId = game.Id;

                Carts.Add(c);
            }

            c.Count += number;

            if(c.Count <= 0) {
                Carts.Remove(c);
            }

            SaveChanges();
        }

        public IEnumerable<Tuple<Cart, Game, ApplicationUser>> TuplizeFor(ApplicationUser u) {
            Game[] garms = games.Games.ToArray();
            ApplicationUser[] uzerz = users.Users.ToArray();
            Cart[] crtz = Carts.Where(x => x.UserId == u.Id).ToArray();

            List<Tuple<Cart, Game, ApplicationUser>> tplz = new List<Tuple<Cart, Game, ApplicationUser>>(crtz.Count());
            foreach(Cart x in crtz) {
                tplz.Add(
                    new Tuple<Cart, Game, ApplicationUser>(
                    x,
                    garms.Where(y => y.Id == x.GameId).FirstOrDefault(),
                    uzerz.Where(y => y.Id == x.UserId).FirstOrDefault())
                );
            }
            return tplz;
        }

        public new void Dispose() {
            users.Dispose();
            games.Dispose();
            base.Dispose();
        }

        public IEnumerable<Game> GetGames(ApplicationUser forUser) {
            var gIds = Carts
                .Where(y => y.UserId == forUser.Id)
                .Select(y => y.GameId);

            return games.Games.Where(x =>
                gIds.Contains(x.Id)
            );
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using VaporStore.Data.Dto;
using VaporStore.Data.Dto.Import;
using VaporStore.Data.Models;
using VaporStore.Inftastructire;

namespace VaporStore.DataProcessor
{
    using System;
    using Data;

    public static class Deserializer
    {
        private static ICollection<ValidationResult> validationTestResults;

        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            var gameDtos = JsonConvert.DeserializeObject<ImportGamesDto[]>(jsonString);

            var validGames = new List<Game>();
            foreach (var gameDto in gameDtos)
            {
                GenericValidator.TryValidate(gameDto, out validationTestResults);

                if (validationTestResults.Count != 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var game = CreateGame(gameDto, context);
                context.Games.Add(game);
                context.SaveChanges();

                sb.AppendLine($"Added {game.Name} ({game.Genre.Name}) with {game.GameTags.Count} tags");
            }

            return sb.ToString().TrimEnd();
        }

        private static Game CreateGame(ImportGamesDto gameDto, VaporStoreDbContext context)
        {
            var game = new Game();
            game.Name = gameDto.Name;
            game.Price = gameDto.Price;
            game.ReleaseDate = DateTime.ParseExact(gameDto.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var devInBase = context.Developers.FirstOrDefault(d => d.Name == gameDto.Developer);
            game.Developer = devInBase ?? new Developer()
            {
                Name = gameDto.Developer
            };
            var genreInBase = context.Genres.FirstOrDefault(g => g.Name == gameDto.Genre);
            game.Genre = genreInBase ?? new Genre
            {
                Name = gameDto.Genre
            };

            game.GameTags = CreateGameTagCollection(gameDto.Tags, context, game);

            return game;
        }

        private static ICollection<GameTag> CreateGameTagCollection(IEnumerable gameDtoTags,
            VaporStoreDbContext context, Game game)
        {
            var gameTags = new List<GameTag>();

            foreach (var gameTag in gameDtoTags)
            {
                var currentGameTag = new GameTag()
                {
                    Game = game,
                    Tag = context.Tags.FirstOrDefault(t => t.Name == gameTag.ToString()) ?? new Tag()
                    {
                        Name = gameTag.ToString()
                    }
                };
                gameTags.Add(currentGameTag);
            }

            return gameTags;
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            var userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(jsonString);

            foreach (var userDto in userDtos)
            {
                bool isInvalidCardType = ValidateCardType(userDto.Cards);
                GenericValidator.TryValidate(userDto, out validationTestResults);
                var temp = validationTestResults;
                bool isInvalidCard = ValidateCards(userDto.Cards);
                if (temp.Count != 0
                    || userDto.Cards.Count == 0
                    || isInvalidCard
                    || isInvalidCardType)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var user = CreateUser(userDto, context);

                context.Users.Add(user);
                context.SaveChanges();
                sb.AppendLine($"Imported {user.Username} with {user.Cards.Count} cards");
            }

            return sb.ToString().TrimEnd();
        }

        private static bool ValidateCardType(ICollection<CardDto> userDtoCards)
        {
            foreach (var cardType in userDtoCards)
            {
                var test = Enum.TryParse<CardType>(cardType.Type, out var cardT);
                if (test)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool ValidateCards(ICollection<CardDto> userDtoCards)
        {
            foreach (var cardDto in userDtoCards)
            {
                GenericValidator.TryValidate(cardDto, out validationTestResults);
                if (validationTestResults.Count == 0)
                {
                    return false;
                }
            }

            return true;
        }

        private static User CreateUser(ImportUserDto importUserDto, VaporStoreDbContext context)
        {
            var user = new User();
            user.Username = importUserDto.Username;
            user.FullName = importUserDto.FullName;
            user.Age = importUserDto.Age;
            user.Email = importUserDto.Email;
            user.Cards = CreateCardCollection(user, importUserDto.Cards, context);
            return user;
        }

        private static ICollection<Card> CreateCardCollection(User user, ICollection<CardDto> userDtoCards,
            VaporStoreDbContext context)
        {
            var userCards = new List<Card>();
            foreach (var userDtoCard in userDtoCards)
            {
                var currentCard = new Card()
                {
                    Number = userDtoCard.Number,
                    Cvc = userDtoCard.Cvc,
                    Type = Enum.Parse<CardType>(userDtoCard.Type)
                };

                userCards.Add(currentCard);
            }

            return userCards;
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlSerializer serializer =
                new XmlSerializer(typeof(ImportPurchaseDto[]), new XmlRootAttribute("Purchases"));

            var purchaseDtos = (ImportPurchaseDto[]) serializer.Deserialize(new StringReader(xmlString));

            var validPurchases = new List<Game>();
            foreach (var purchaseDto in purchaseDtos)
            {
                GenericValidator.TryValidate(purchaseDto, out validationTestResults);
                bool isValid = IsValidEnumPurchaseType(purchaseDto.Type);
                if (validationTestResults.Count != 0 ||isValid == false)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var purchase = CreatePurchase(purchaseDto, context);
                if (purchase.Game == null || purchase.Card == null || purchase.Type == 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                context.Purchases.Add(purchase);
                context.SaveChanges();

                sb.AppendLine($"Imported {purchase.Game.Name} for {purchase.Card.User.Username}");
            }

            return sb.ToString().TrimEnd();
        }

        private static bool IsValidEnumPurchaseType(string purchaseDtoType)
        {
            return Enum.TryParse<PurchaseType>(purchaseDtoType, out var test);
        }

        private static Purchase CreatePurchase(ImportPurchaseDto purchaseDto, VaporStoreDbContext context)
        {
            var purchase = new Purchase();
            purchase.Date = DateTime.ParseExact(purchaseDto.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            purchase.Game = context.Games.FirstOrDefault(x => x.Name == purchaseDto.Title);
            purchase.Card = context.Cards.FirstOrDefault(x => x.Number.ToString() == purchaseDto.Card);
            purchase.ProductKey = purchaseDto.Key;
            purchase.Type = Enum.Parse<PurchaseType>(purchaseDto.Type);

            return purchase;
        }
    }
}
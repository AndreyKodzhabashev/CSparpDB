﻿using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks.Dataflow;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Remotion.Linq.Clauses;
using VaporStore.Data.Dto;
using VaporStore.Data.Dto.Export;
using VaporStore.Data.Models;
using Formatting = Newtonsoft.Json.Formatting;

namespace VaporStore.DataProcessor
{
    using System;
    using Data;

    public static class Serializer
    {
        private const string expectedXml = "<Users><User username=\"fstoter\"><Purchases><Purchase><Card>9466 9592 0503 1368</Card><Cvc>753</Cvc><Date>2016-06-11 10:53</Date><Game title=\"Team Fortress 2\"><Genre>Action</Genre><Price>0</Price></Game></Purchase><Purchase><Card>9466 9592 0503 1368</Card><Cvc>753</Cvc><Date>2016-10-24 13:26</Date><Game title=\"Far Cry 5\"><Genre>Action</Genre><Price>59.99</Price></Game></Purchase><Purchase><Card>9466 9592 0503 1368</Card><Cvc>753</Cvc><Date>2017-09-16 16:04</Date><Game title=\"MONSTER HUNTER: WORLD\"><Genre>Action</Genre><Price>59.99</Price></Game></Purchase></Purchases><TotalSpent>119.98</TotalSpent></User><User username=\"aputland\"><Purchases><Purchase><Card>2263 5851 7894 9441</Card><Cvc>192</Cvc><Date>2016-11-27 12:17</Date><Game title=\"Pro Cycling Manager 2018\"><Genre>Simulation</Genre><Price>39.99</Price></Game></Purchase><Purchase><Card>7660 9400 3206 5606</Card><Cvc>600</Cvc><Date>2017-12-12 04:37</Date><Game title=\"Rocket League\"><Genre>Action</Genre><Price>12.59</Price></Game></Purchase><Purchase><Card>7660 9400 3206 5606</Card><Cvc>600</Cvc><Date>2017-12-29 19:37</Date><Game title=\"Tom Clancy\'s Ghost Recon Wildlands\"><Genre>Action</Genre><Price>59.99</Price></Game></Purchase></Purchases><TotalSpent>112.57</TotalSpent></User><User username=\"bgunston\"><Purchases><Purchase><Card>3762 5646 9250 3278</Card><Cvc>111</Cvc><Date>2016-01-28 16:06</Date><Game title=\"Factorio\"><Genre>Casual</Genre><Price>25</Price></Game></Purchase><Purchase><Card>9329 2624 0151 4535</Card><Cvc>689</Cvc><Date>2016-07-21 23:51</Date><Game title=\"Football Manager 2018\"><Genre>Simulation</Genre><Price>54.99</Price></Game></Purchase><Purchase><Card>9329 2624 0151 4535</Card><Cvc>689</Cvc><Date>2016-09-11 22:27</Date><Game title=\"NARUTO SHIPPUDEN: Ultimate Ninja STORM 4\"><Genre>Action</Genre><Price>29.99</Price></Game></Purchase></Purchases><TotalSpent>109.98</TotalSpent></User><User username=\"cbelchamber\"><Purchases><Purchase><Card>6842 0546 4406 5606</Card><Cvc>538</Cvc><Date>2018-01-15 15:50</Date><Game title=\"NBA 2K18\"><Genre>Simulation</Genre><Price>49.99</Price></Game></Purchase><Purchase><Card>6842 0546 4406 5606</Card><Cvc>538</Cvc><Date>2018-02-24 20:06</Date><Game title=\"Call of Duty: Black Ops III\"><Genre>Action</Genre><Price>59.99</Price></Game></Purchase></Purchases><TotalSpent>109.98</TotalSpent></User><User username=\"mgillicuddy\"><Purchases><Purchase><Card>9924 7778 1587 0277</Card><Cvc>603</Cvc><Date>2016-06-26 12:40</Date><Game title=\"NARUTO SHIPPUDEN: Ultimate Ninja STORM 4\"><Genre>Action</Genre><Price>29.99</Price></Game></Purchase><Purchase><Card>4499 3123 4695 9542</Card><Cvc>444</Cvc><Date>2016-11-18 05:52</Date><Game title=\"Vampyr\"><Genre>Action</Genre><Price>49.99</Price></Game></Purchase><Purchase><Card>4499 3123 4695 9542</Card><Cvc>444</Cvc><Date>2018-06-19 16:00</Date><Game title=\"Warframe\"><Genre>Violent</Genre><Price>0</Price></Game></Purchase></Purchases><TotalSpent>79.98</TotalSpent></User><User username=\"hrichardson\"><Purchases><Purchase><Card>4082 9960 2674 5955</Card><Cvc>598</Cvc><Date>2016-07-04 14:35</Date><Game title=\"Crash Bandicoot N. Sane Trilogy\"><Genre>Action</Genre><Price>39.99</Price></Game></Purchase><Purchase><Card>5811 6621 2962 1020</Card><Cvc>375</Cvc><Date>2017-01-18 02:48</Date><Game title=\"Factorio\"><Genre>Casual</Genre><Price>25</Price></Game></Purchase><Purchase><Card>4082 9960 2674 5955</Card><Cvc>598</Cvc><Date>2017-08-02 09:16</Date><Game title=\"Neverwinter\"><Genre>Adventure</Genre><Price>0</Price></Game></Purchase></Purchases><TotalSpent>64.99</TotalSpent></User><User username=\"aruthven\"><Purchases><Purchase><Card>5208 8381 5687 8508</Card><Cvc>624</Cvc><Date>2017-01-22 09:33</Date><Game title=\"The Crew 2\"><Genre>Action</Genre><Price>59.99</Price></Game></Purchase></Purchases><TotalSpent>59.99</TotalSpent></User><User username=\"atobin\"><Purchases><Purchase><Card>6975 1775 3435 4897</Card><Cvc>857</Cvc><Date>2018-01-11 06:49</Date><Game title=\"The Crew 2\"><Genre>Action</Genre><Price>59.99</Price></Game></Purchase></Purchases><TotalSpent>59.99</TotalSpent></User><User username=\"mgraveson\"><Purchases><Purchase><Card>7790 7962 4262 5606</Card><Cvc>966</Cvc><Date>2016-07-05 17:01</Date><Game title=\"MONSTER HUNTER: WORLD\"><Genre>Action</Genre><Price>59.99</Price></Game></Purchase></Purchases><TotalSpent>59.99</TotalSpent></User><User username=\"bgraith\"><Purchases><Purchase><Card>4611 7969 4921 6749</Card><Cvc>519</Cvc><Date>2017-08-29 17:59</Date><Game title=\"Warhammer 40,000: Gladius - Relics of War\"><Genre>Strategy</Genre><Price>37.99</Price></Game></Purchase><Purchase><Card>5962 2881 2375 4209</Card><Cvc>022</Cvc><Date>2018-08-08 08:10</Date><Game title=\"Human: Fall Flat\"><Genre>Adventure</Genre><Price>14.99</Price></Game></Purchase></Purchases><TotalSpent>52.98</TotalSpent></User><User username=\"mdickson\"><Purchases><Purchase><Card>5317 5177 3653 0084</Card><Cvc>963</Cvc><Date>2016-01-02 19:25</Date><Game title=\"FINAL FANTASY XIV Online\"><Genre>Massively Multiplayer</Genre><Price>9.99</Price></Game></Purchase><Purchase><Card>5317 5177 3653 0084</Card><Cvc>963</Cvc><Date>2017-01-14 04:09</Date><Game title=\"Planet Coaster\"><Genre>Simulation</Genre><Price>37.99</Price></Game></Purchase><Purchase><Card>5317 5177 3653 0084</Card><Cvc>963</Cvc><Date>2018-04-19 06:26</Date><Game title=\"Team Fortress 2\"><Genre>Action</Genre><Price>0</Price></Game></Purchase></Purchases><TotalSpent>47.98</TotalSpent></User><User username=\"bcorcut\"><Purchases><Purchase><Card>7460 6498 2791 0231</Card><Cvc>068</Cvc><Date>2016-07-26 17:20</Date><Game title=\"The Elder Scrolls V: Skyrim Special Edition\"><Genre>RPG</Genre><Price>39.99</Price></Game></Purchase></Purchases><TotalSpent>39.99</TotalSpent></User><User username=\"cgara\"><Purchases><Purchase><Card>5103 9356 9768 6854</Card><Cvc>493</Cvc><Date>2017-05-11 08:15</Date><Game title=\"Crash Bandicoot N. Sane Trilogy\"><Genre>Action</Genre><Price>39.99</Price></Game></Purchase></Purchases><TotalSpent>39.99</TotalSpent></User><User username=\"kcarroll\"><Purchases><Purchase><Card>2844 3311 3796 4444</Card><Cvc>137</Cvc><Date>2017-05-17 19:38</Date><Game title=\"DARK SOULS: REMASTERED\"><Genre>Action</Genre><Price>39.99</Price></Game></Purchase></Purchases><TotalSpent>39.99</TotalSpent></User><User username=\"vsjollema\"><Purchases><Purchase><Card>7815 5830 0145 0448</Card><Cvc>249</Cvc><Date>2016-09-13 03:51</Date><Game title=\"Squad\"><Genre>Action</Genre><Price>36.99</Price></Game></Purchase></Purchases><TotalSpent>36.99</TotalSpent></User><User username=\"klife\"><Purchases><Purchase><Card>2962 0872 0998 4724</Card><Cvc>638</Cvc><Date>2017-08-03 12:48</Date><Game title=\"Raft\"><Genre>Single-player</Genre><Price>19.99</Price></Game></Purchase><Purchase><Card>0540 4834 3653 5943</Card><Cvc>943</Cvc><Date>2018-06-06 20:34</Date><Game title=\"Tom Clancy\'s Rainbow Six Siege\"><Genre>Action</Genre><Price>14.99</Price></Game></Purchase></Purchases><TotalSpent>34.98</TotalSpent></User><User username=\"ehellard\"><Purchases><Purchase><Card>3013 7441 5769 1224</Card><Cvc>703</Cvc><Date>2016-09-27 03:30</Date><Game title=\"Soul at Stake\"><Genre>Violent</Genre><Price>14.99</Price></Game></Purchase><Purchase><Card>2643 8516 1644 3240</Card><Cvc>808</Cvc><Date>2016-10-21 09:44</Date><Game title=\"Dungeon Warfare 2\"><Genre>Indie</Genre><Price>12.49</Price></Game></Purchase><Purchase><Card>2643 8516 1644 3240</Card><Cvc>808</Cvc><Date>2018-09-08 17:36</Date><Game title=\"Warframe\"><Genre>Violent</Genre><Price>0</Price></Game></Purchase></Purchases><TotalSpent>27.48</TotalSpent></User><User username=\"rabbison\"><Purchases><Purchase><Card>5747 3965 9959 7596</Card><Cvc>463</Cvc><Date>2018-05-11 04:05</Date><Game title=\"Dungeon Warfare 2\"><Genre>Indie</Genre><Price>12.49</Price></Game></Purchase><Purchase><Card>5747 3965 9959 7596</Card><Cvc>463</Cvc><Date>2018-07-20 18:52</Date><Game title=\"Guns, Gore and Cannoli 2\"><Genre>Action</Genre><Price>12.99</Price></Game></Purchase></Purchases><TotalSpent>25.48</TotalSpent></User><User username=\"fdedam\"><Purchases><Purchase><Card>9574 1800 3833 2972</Card><Cvc>512</Cvc><Date>2017-10-20 11:26</Date><Game title=\"Dead by Daylight\"><Genre>Violent</Genre><Price>19.99</Price></Game></Purchase></Purchases><TotalSpent>19.99</TotalSpent></User><User username=\"dsharple\"><Purchases><Purchase><Card>6752 6869 9870 9732</Card><Cvc>719</Cvc><Date>2017-04-12 06:39</Date><Game title=\"Banished\"><Genre>Indie</Genre><Price>18.99</Price></Game></Purchase><Purchase><Card>6752 6869 9870 9732</Card><Cvc>719</Cvc><Date>2017-07-20 14:22</Date><Game title=\"Trove\"><Genre>Action</Genre><Price>0</Price></Game></Purchase></Purchases><TotalSpent>18.99</TotalSpent></User><User username=\"spaprotny\"><Purchases><Purchase><Card>9185 2070 3009 4543</Card><Cvc>518</Cvc><Date>2016-11-15 18:07</Date><Game title=\"Banished\"><Genre>Indie</Genre><Price>18.99</Price></Game></Purchase><Purchase><Card>6777 2480 1837 5824</Card><Cvc>533</Cvc><Date>2017-04-01 17:27</Date><Game title=\"Paladins\"><Genre>Action</Genre><Price>0</Price></Game></Purchase></Purchases><TotalSpent>18.99</TotalSpent></User><User username=\"wskep\"><Purchases><Purchase><Card>0327 7877 3023 9451</Card><Cvc>939</Cvc><Date>2018-07-10 14:43</Date><Game title=\"Soul at Stake\"><Genre>Violent</Genre><Price>14.99</Price></Game></Purchase></Purchases><TotalSpent>14.99</TotalSpent></User><User username=\"kroderighi\"><Purchases><Purchase><Card>7036 3344 0149 7880</Card><Cvc>094</Cvc><Date>2016-12-25 12:01</Date><Game title=\"Stardew Valley\"><Genre>Indie</Genre><Price>13.99</Price></Game></Purchase></Purchases><TotalSpent>13.99</TotalSpent></User><User username=\"bfrontczak\"><Purchases><Purchase><Card>9104 6735 7127 3894</Card><Cvc>985</Cvc><Date>2018-01-28 00:34</Date><Game title=\"Shadowverse CCG\"><Genre>Free to Play</Genre><Price>0</Price></Game></Purchase></Purchases><TotalSpent>0</TotalSpent></User><User username=\"kmertgen\"><Purchases><Purchase><Card>1268 2352 8506 0500</Card><Cvc>693</Cvc><Date>2017-10-06 17:13</Date><Game title=\"Warframe\"><Genre>Violent</Genre><Price>0</Price></Game></Purchase></Purchases><TotalSpent>0</TotalSpent></User><User username=\"svannini\"><Purchases><Purchase><Card>1642 7380 2920 7598</Card><Cvc>908</Cvc><Date>2017-08-04 12:34</Date><Game title=\"Trove\"><Genre>Action</Genre><Price>0</Price></Game></Purchase></Purchases><TotalSpent>0</TotalSpent></User></Users>";
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            var games = context.Genres
                .Where(x => genreNames.Contains(x.Name))
                .Select(g => new
                {
                    Id = g.Id,
                    Genre = g.Name,
                    Games = g.Games
                        .Where(d => d.Purchases.Any())
                        .Select(game => new
                        {
                            Id = game.Id,
                            Title = game.Name,
                            Developer = game.Developer.Name,
                            Tags = string.Join(", ", game.GameTags.Select(t => t.Tag.Name).ToArray()),
                            Players = game.Purchases.Count

                        })
                        .OrderByDescending(game => game.Players)
                        .ThenBy(game => game.Id).ToArray(),
                    TotalPlayers = g.Games.Sum(x => x.Purchases.Count)
                })
                .OrderByDescending(game => game.TotalPlayers)
                .ThenBy(game => game.Id)
                .ToArray();

            var result = JsonConvert.SerializeObject(games, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });

            return result;
        }


        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
        {
            
            var purchaseType = Enum.Parse<PurchaseType>(storeType);

            var users = context
                .Users
                .Select(x => new ExportUserDto
                {
                    Username = x.Username,
                    Purchases = x.Cards
                        .SelectMany(p => p.Purchases)
                        .Where(t => t.Type == purchaseType)
                        .Select(p => new ExportPurchasesDto
                        {
                            Card = p.Card.Number,
                            Cvc = p.Card.Cvc,
                            Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                            Game = new ExportGameDto
                            {
                                Genre = p.Game.Genre.Name,
                                Title = p.Game.Name,
                                Price = p.Game.Price!= 0? p.Game.Price:0
                            }
                        })
                        .OrderBy(d => d.Date)
                        .ToArray(),
                    TotalSpent = x.Cards.SelectMany(p => p.Purchases)
                        .Where(t => t.Type == purchaseType)
                        .Sum(p => p.Game.Price)
                })
                .Where(p => p.Purchases.Any())
                .OrderByDescending(t => t.TotalSpent)
                .ThenBy(u => u.Username)
                .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(ExportUserDto[]), new XmlRootAttribute("Users"));
            var namespaces = new XmlSerializerNamespaces(new[]
            {
                XmlQualifiedName.Empty,
            });

            var sb = new StringBuilder();
            xmlSerializer.Serialize(new StringWriter(sb), users, namespaces);
            var expected = XDocument.Parse(expectedXml);
            //Console.WriteLine(expected);
            //Console.WriteLine();
            var tesmp = XDocument.Parse(sb.ToString().TrimEnd());
            //Console.WriteLine(tesmp);
            var result = sb.ToString().TrimEnd();

            return result;
        }
    }
}
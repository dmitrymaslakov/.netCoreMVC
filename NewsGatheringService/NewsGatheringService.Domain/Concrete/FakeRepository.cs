using NewsGatheringService.Core.Abstract;
using NewsGatheringService.Data.Entities;
using NewsGatheringService.Domain.Abstract;
using NewsGatheringService.Domain.Onliner;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NewsGatheringService.Domain.Concrete
{
    public class FakeRepository : IRepository
    {

        public FakeRepository()
        {
            #region MyRegion

            /*var random = new Random();

            string dataComments = File.ReadAllText(@"F:\Разное\Учёба\Программирование Си шарп\It-academy\Projects\NewsGatheringService\NewsGatheringService.Domain\StaticFile\Comments.json");
            string dataNews = File.ReadAllText(@"F:\Разное\Учёба\Программирование Си шарп\It-academy\Projects\NewsGatheringService\NewsGatheringService.Domain\StaticFile\News.json");
            string dataNewsStructure = File.ReadAllText(@"F:\Разное\Учёба\Программирование Си шарп\It-academy\Projects\NewsGatheringService\NewsGatheringService.Domain\StaticFile\NewsStructure.json");
            string dataUsers = File.ReadAllText(@"F:\Разное\Учёба\Программирование Си шарп\It-academy\Projects\NewsGatheringService\NewsGatheringService.Domain\StaticFile\Users.json");
            string dataNewsHeaderImage = File.ReadAllText(@"F:\Разное\Учёба\Программирование Си шарп\It-academy\Projects\NewsGatheringService\NewsGatheringService.Domain\StaticFile\NewsHeaderImage.json");
            string dataProfilePicture = File.ReadAllText(@"F:\Разное\Учёба\Программирование Си шарп\It-academy\Projects\NewsGatheringService\NewsGatheringService.Domain\StaticFile\ProfilePicture.json");

            var comments = JsonSerializer.Deserialize<List<Comment>>(dataComments);
            var news = JsonSerializer.Deserialize<List<News>>(dataNews);
            var newsStructure = JsonSerializer.Deserialize<List<NewsStructure>>(dataNewsStructure);
            var users = JsonSerializer.Deserialize<List<User>>(dataUsers);
            var newsHeaderImage = JsonSerializer.Deserialize<string[]>(dataNewsHeaderImage);
            var profilePicture = JsonSerializer.Deserialize<string[]>(dataProfilePicture);

            Comments = comments;
            NewsStructure = newsStructure;

            for (int i = 0; i < newsHeaderImage.Length; i++)
            {
                var imageUrl = newsHeaderImage[i];
                var stream = new WebClient().OpenRead(imageUrl);
                var data = new byte[0];
                using (var streamReader = new MemoryStream())
                {
                    stream.CopyTo(streamReader);
                    data = streamReader.ToArray();
                }
                news[i].NewsHeaderImage = data;
            }

            for (int i = 0; i < profilePicture.Length; i++)
            {
                var pictureUrl = profilePicture[i];
                var stream = new WebClient().OpenRead(pictureUrl);
                var data = new byte[0];
                using (var streamReader = new MemoryStream())
                {
                    stream.CopyTo(streamReader);
                    data = streamReader.ToArray();
                }
                users[i].ProfilePicture = data;
            }

            Users = users;
            for (int i = 0; i < news.Count; i++)
            {
                Comments.ToList()[i].User = Users.ToList()[i];

                news[i].Comments = Comments;
                news[i].NewsStructure = newsStructure[i];

            }
            News = news;

            Subcategories = new List<Subcategory>
            {
                new Subcategory { Id = new Guid(), Name = "В мире"},
                new Subcategory { Id = new Guid(), Name = "В стране"},
                new Subcategory { Id = new Guid(), Name = "Футбол"},
                new Subcategory { Id = new Guid(), Name = "Хоккей"},
                new Subcategory { Id = new Guid(), Name = "Волейбол"},
                new Subcategory { Id = new Guid(), Name = "Кино"},
                new Subcategory { Id = new Guid(), Name = "Музыка"},
                new Subcategory { Id = new Guid(), Name = "Концерты"},
                new Subcategory { Id = new Guid(), Name = "Театр"},
                new Subcategory { Id = new Guid(), Name = "Гаджеты"},
                new Subcategory { Id = new Guid(), Name = "Игры"},
                new Subcategory { Id = new Guid(), Name = "ПО"},
                new Subcategory { Id = new Guid(), Name = "Авто"},
                new Subcategory { Id = new Guid(), Name = "Вело"},
                new Subcategory { Id = new Guid(), Name = "Авиа"},
                new Subcategory { Id = new Guid(), Name = "Водный транспорт"},
            };

            Categories = new List<Category> {
                    new Category { Id = new Guid(), Name = "Главное", Subcategories = Subcategories.Take(2)},
                    new Category { Id = new Guid(), Name = "Спорт", Subcategories = Subcategories.Skip(2).Take(3)},
                    new Category { Id = new Guid(), Name = "Культура", Subcategories = Subcategories.Skip(5).Take(4)},
                    new Category { Id = new Guid(), Name = "Технологии", Subcategories = Subcategories.Skip(9).Take(3)},
                    new Category { Id = new Guid(), Name = "Люди"},
                    new Category { Id = new Guid(), Name = "Здоровье"},
                    new Category { Id = new Guid(), Name = "Техника", Subcategories = Subcategories.Skip(12)},
                    new Category { Id = new Guid(), Name = "Отдых"}
                };*/
            #endregion

           /* var sourceNewsParser = new ParserWorker<IEnumerable<string>>(new OnlinerNewsUrlSourceParser(), "https://www.onliner.by/");
            var newsUrlSources = sourceNewsParser.WorkerAsync().Result;

            var newsList = new HashSet<News>();
            foreach (var newsUrlSource in newsUrlSources)
            {
                if (string.IsNullOrEmpty(newsUrlSource)) continue;
                var newsParser = new ParserWorker<News>(new OnlinerNewsParser(), newsUrlSource);
                var news = newsParser.WorkerAsync().Result;
                newsList.Add(news);
            }

            Category category = null;
            Subcategory subcategory = null;

            foreach (var newsFromParse in newsList)
            {

                category = new Category
                {
                    Id = Guid.NewGuid(),
                    Name = newsFromParse.Category.Name
                };

                if (Categories == null)
                {
                    Categories = new HashSet<Category> { category };
                }
                else
                {

                    if (!Categories.Select(c => c.Name).Contains(newsFromParse.Category.Name))
                        Categories = Categories.Append(category);
                    else
                    {
                        category = Categories.Where(c => c.Name.Equals(category.Name)).FirstOrDefault();
                    }
                }


                subcategory = new Subcategory
                {
                    Id = Guid.NewGuid(),
                    Name = newsFromParse.Subcategory.Name,
                    CategoryId = category.Id
                };

                if (Subcategories == null)
                {
                    Subcategories = new HashSet<Subcategory> { subcategory };
                }
                else
                {
                    if (!Subcategories.Select(c => c.Name).Contains(newsFromParse.Subcategory.Name))
                        Subcategories = Subcategories.Append(subcategory);
                    else
                    {
                        subcategory = Subcategories.Where(s => s.Name.Equals(subcategory.Name)).FirstOrDefault();
                    }
                }


                var news = new News
                {
                    Id = Guid.NewGuid(),
                    Author = newsFromParse.Author,
                    Date = newsFromParse.Date,
                    NewsHeaderImage = newsFromParse.NewsHeaderImage,
                    Reputation = newsFromParse.Reputation,
                    Source = newsFromParse.Source,
                    CategoryId = category.Id,
                    SubcategoryId = subcategory.Id
                };
                var newsStructure = new NewsStructure
                {
                    Id = Guid.NewGuid(),
                    Headline = newsFromParse.NewsStructure.Headline,
                    Lead = newsFromParse.NewsStructure.Lead,
                    Body = newsFromParse.NewsStructure.Body,
                    Background = newsFromParse.NewsStructure.Background,
                    NewsId = news.Id
                };



                news.NewsStructure = newsStructure;
                news.Category = category;
                news.Subcategory = subcategory;

                News = News == null ? new HashSet<News> { news } : News.Append(news);
                NewsStructures = NewsStructures == null ? new HashSet<NewsStructure> { newsStructure } : NewsStructures.Append(newsStructure);

                Categories = Categories.Select(c =>
                {
                    if (c.Subcategories == null)
                        c.Subcategories = new HashSet<Subcategory> { subcategory };
                    else if (c.Id.Equals(subcategory.CategoryId))
                    {
                        if (!c.Subcategories.Select(c => c.Name).Contains(subcategory.Name))
                            c.Subcategories = c.Subcategories.Append(subcategory);
                    }

                    return c;
                });

            }

            Categories = Categories.Select(c =>
            {
                c.News = News.Where(n => n.CategoryId.Equals(c.Id));
                return c;
            });

            Subcategories = Subcategories.Select(s =>
            {
                s.News = News.Where(n => n.SubcategoryId.Equals(s.Id));
                return s;
            });
           */
        }

        public IUnitOfWork UnitOfWork { get; set; }
    }
}

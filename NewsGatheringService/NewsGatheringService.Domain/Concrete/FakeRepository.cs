using Moq;
using NewsGatheringService.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NewsGatheringService.Domain.Concrete
{
    public class FakeRepository : IRepository
    {
        public FakeRepository()
        {
            var random = new Random();

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
                };

        }

        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<Comment> Comments { get; set; }

        public IEnumerable<News> News { get; set; }

        public IEnumerable<NewsStructure> NewsStructure { get; set; }

        public IEnumerable<Role> Roles { get; set; }

        public IEnumerable<Subcategory> Subcategories { get; set; }

        public IEnumerable<UserRole> UserRoles => throw new NotImplementedException();

        public IEnumerable<User> Users { get; set; }

        public object DeleteMerch(int entityId)
        {
            throw new NotImplementedException();
        }

        public void SaveMerch(object entity)
        {
            throw new NotImplementedException();
        }
    }
}

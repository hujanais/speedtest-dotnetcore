using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace speedtest_dotnetcore
{
    public class Entity
    {
        public ObjectId Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public double Ping { get; set; }
        public double DownloadMbitsPerSec { get; set; }
        public double UploadMbitsPerSec { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class Mongoose
    {
        private MongoClient mongoClient;
        private IMongoDatabase db;
        private IMongoCollection<Entity> collection = null;

        public Mongoose(string mongodb_url, string db_name, string collectionName)
        {
            mongoClient = new MongoClient(mongodb_url);
            this.db = mongoClient.GetDatabase(db_name);
            bool isDBLive = this.db.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(5000);

            if (isDBLive)
            {
                this.collection = this.db.GetCollection<Entity>(collectionName);
                Console.WriteLine("MongoDB connected");
            } else
            {
                throw new Exception("MongoDB connection failed.");
            }
        }

        /// <summary>
        /// Add data to the database.
        /// </summary>
        /// <param name="timeStamp">timestamp in utc time</param>
        /// <param name="ping">time in ms</param>
        /// <param name="download">download in MBitsPerSec</param>
        /// <param name="upload">upload in MBitsPerSec</param>
        /// <param name="errorMessage">error message if available</param>
        public void AddData(DateTime timeStamp, double ping, double download, double upload, string errorMessage)
        {
            var document = new Entity()
            {
                TimeStamp = timeStamp,
                DownloadMbitsPerSec = download,
                UploadMbitsPerSec = upload,
                Ping = ping,
                ErrorMessage = errorMessage
            };

            this.collection.InsertOne(document);
        }
    }
}

using TRXParser.Properties;
using System;
using System.IO;
using System.Text;
using System.Data;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TRXParser
{
    static class Program
    {
        private static DataTable ResultTable;
        private const string OK = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADAAAAAwCAYAAABXAvmHAAAABmJLR0QA/wD/AP+gvaeTAAAErklEQVRoge2ZTUgcZxjH/8/MrAeJtFhXsn600hLd3VgLXbUUUtCQi8XFrGZNW1vooaQVDfReClsovZe6NubQk4eoUVd2TSmE4LEYhRYbbYT0kFiVbpQGF0V3Z54eojHdeXdnZmf0Ev+3fd6P+f3nfXbeL+BEJ3qxRU50Eh4NyztKppklrVViCmiAlwAPAacAgIEUmFaJ+L5GPC+p8p3AQsNsJBLR7D7bloGOWEe1pkl9DHwM4kqLzVdAPKyqcvTnromVQhkKMtA2GnYrrvS3DHwKoKjQh+9rD8Q/kax9HQ/GH1ttbNlAMNbxETP9AKDUalsDbTBT/3Tn5A0rjUwbCAxdcXncyUEQf2adzbyYaWg96b46//n1tJn6pgwE48Fizsg3AbTZojOvW6So4Xgwvm1UUTKqEBi64jpmeAB4nzPyVHg0bPj/MjTgcScHcbzwB7qwU7T3vVGlvCnUPhHqAfGwc0wFiPiDxMWpkZzFuQpCE6FX0sR/Aig7EjDz2iRFrcv1ic2ZQhni73CM8GUlxbmKSrWM/E2uQuEItI13VsmS9gD2JylTqj1diu7mevz6YAW37/0lqrJHinomHow/zC4QjoBC3I9jhlckCefOvIoLZ18XVStiVeoVFegMRCIRiYl7nAYVyedx43Lzm1CkQ4x336iGW5ROmvRJeDQsZ4d1BuYafn8HQJWzqHrVni5FV5MPsnSYxarGuHl3EcktwfxFXLnjSgeywzoDLGmtDrPq5K8o0735A/iltWTOdsR0PjumM0Ca1OgUqEj+ijJ0NZ4Vvvl88ADAgPEIAKi1TZlDduABgIjrsmN6A8Qes0B5vt06+TxuW/AAwICOTTQCp8x01uKtQW9rE3wet2Fdf0UZLjX5bcHvqyQ7YLiYE6nFW4MWbw1kiXCpyZ/XhN20MZLIQCpfA3dJMd6rfe3Z73wmjgB+KzugN8C0lq+H5NY2RmYXkNEODxRkidDd7Ed9VflRwoMAHZtoBJaNOlpe38To7B//M0FE6Ar4UF9VfmRpw0z3s2OiiWzOTGfL65sYv7sEVeNnMSJC6G2fEH587p7tnBex6QxIqnzHbIdLa0lhOone/OKq5RMTnURsOgOBhYZZAI/MdipKpwM5+bUB8DCw0DCfHdSt7mZmZrj2w7pygM6Z7XkjtYP1J1vwVbgh0dO37zA8AAxe7712OzsonAdUVY4C2LPS+/MjcQTwu1AyUVFBzj1xe6zjRzB9YfVJ/oqnu1Ancv45DSRCsauigpwzcXqv6CsAlikWVx87Db9BippzT5zTwC/dY5vMJHR9nGKm3nyHvnnXQtOdkzeYach5LJMijk53To7lq2K4mCvOKH0AYo5BmRQRT6deevKlUT1DA2PdYyopag+AW46QmVMCstY90zqTMapoajkdD8a31/4pvwjia/bZDEQcTb38b8jMyTRQwAVHe6zjMpgG4PypXZKZ+oxyPluWNzSJi1MjLiYvA4MAdq22F2gXwEA67fJahQdsXvK1x9srocr9YOoBUG2x+SMAw1Ay0UQw8XehDI5cs0YiEWnurd8aiek8AwEirmOgEof76xSYVgAss6TN7V+zzjtxzXqiE73o+g+dpfqwtFDpgQAAAABJRU5ErkJggg==";
        private const string NOK = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADAAAAAwCAYAAABXAvmHAAAABmJLR0QA/wD/AP+gvaeTAAAE30lEQVRoge2ZTWxUVRTHf+e9B5NOWyWoC1LQhQugTU3MQIkJSOkUIhgTXTQY0cQFCwmQuMEoXxas4MdCjRTFhStIILjQIEbFDk1dkLbMCvlMjFFQFwoYO0wzMO8eF0z6Me9N576ZARf0vzz33HP+/3c/zr33wTSmcW9DahFEu7rc0Wu/timsQCWhogsQ5qA0FLJkUP4QlYuIpkUkVbd0cEi6MdXmrkpAdtXiecbIRpQXgaaI3a+gctAx9Mb7B69UyqEiASOrH3+Im16PIC8DMytNXsBNFT7XvL/jvv7031E7RxaQ6Wx7AeVjYHbUvmVwVZFNjX2Dh6N0shagicSMzCx3v8D66NzsoXCg4R9/s6TTt2z8rQToM4n4jaz3Bejq6uhZQvimvs7vkmPpbDlXp5yDJhIz7ip5AGXNjaz3lXa1lF1fZQVkZrn77yr5MWhn5nr9R+W8ppxCmeTidSAHa0cqOhR9vrFv+Eip9pIC/k22PeDABeDBO8LMHteM788vtcWWnEKC7uH/Jw8w23W9XaUaQwVk25fMLRQpO4iD09xq7e40t4KUXX5jUHT9aGfi4dBYYUbj6SZsK6w4xLZsp+7Dz/A6y691b3mSug8OENu6G1zXKgUw0zfehrCGgADtxkFZZxW2QN5b9TQ4DrHXdk4pwlueJLb1LXBdvBUrib2+y16E6Eva1RVwDggY/bFtCTDXJqazsAUv+dQEg0Nsyw689pVB8u0rx8iP2Z7swJnfbMUfaBq9+ksiwKHYoLDCNqI5d4bc29vB98eNrkts6+5JI+EtTxJ7o+hrG0Puvd2Yc2ds02Ec6Si2eQEnWBTlhJcfSAHbiW3rGSdYmE4A3LoZ+PIYQ+7dXeT7vo2QCUQlMAIBrplk209AS6TI3J4Ok0QAqAEFnAkDXSH5QsAzDX3Dj020hO1CcyqITH4gFZxO4tSQPIAEuIUJaKgw+rgIE3JTVK2SPACNxQb7amILKbWCdIq2yhEmIFNpsLF93gkJK+XrhAVGig1hAv6sJPLEIjUG1cnTyaLYTQ0NcAurA5eihg0lbwy5d7rJ9WybvLCrECHIxWJbQIADp6MELUm+sGBDd6cKRagGuQUEiEjKNqDT3Bok7/vk9uyctNvkB1Lk9r4ZFLFlR6RTrIgGuAUE1C0dHAIu2wQ0589O3hYLx4N8/4mAb77/RGAk8gMpzIWzVuSB3+LLhtPFxuAIdGNQOWQVUg2593vIf3/cqkhNnE75kyfI7d0ZXjPCkx0Ke4oM3Ziz7UvmGld/JsKdwFnYYn0wc5pbMefP3j5q2CHnqPtoPHXq90DqUj1GOts+EeUV2wx3EiLsq/9haHNYW8lKbFx3GxD5rfIO4Kqf96PdiQHu/+7UNUVCVd9NqLJhqkffKc9CjX2DhxUO1J6WHQTtbUwNHZ3Kp+xhrmH2IxsVvqwdLWscj/v1r5Zzsn/cHXWPoqypnpcVvq6P+2tr8rgLIMfS2frr/rMqfFo9tzK50N56P/6cDfnb/hExkly8VpB91PrVTvlLYWO5OV+MyBeaxr7hIwYWCLIfyEXtH4KcCPv8Ge6CqOSh2p98HU80GcwmRNcB8yJ2vwx60FGvN6zC2qI2v1m7cUYHFi0yjnSISkLR+SBNjN+vM8AVgUuqnBbRVHzZcLoWv1mnMY17Hf8BBzfcob3PB5UAAAAASUVORK5CYII=";

        /// <summary>
        ///     TRX-Parser for Teams messages
        /// </summary>
        /// <param name="title">URL to the code repository</param>
        /// <param name="repourl">URL to the code repository</param>
        /// <param name="webhook">URL of the Teams WebHook</param>
        /// <param name="resulturl">URL to the test result page</param>
        /// <param name="searchpath">File path with the TRX files</param>
        /// <param name="deltrx">Set if TRX files should deleted after parsing</param>
        /// <param name="oklimit">The percentage above which a test is considered accepted?</param>
        /// <param name="debug">Set true to write debug infos inside the console</param>
        static int Main
        (
            string title = null,
            string repourl = null,
            string webhook = null,
            string resulturl = null,
            string searchpath = null,
            bool deltrx = false,
            int oklimit = 100,
            bool debug = false
        )
        {
            #region Validate parameters
                var _title = title
                    ?? Environment.GetEnvironmentVariable("TRXPARSER_TITLE")
                    ?? "Title argument not set!";

                var _oklimit = (oklimit==0)
                    ? int.Parse(Environment.GetEnvironmentVariable("TRXPARSER_OKLIMIT")):oklimit;

                var _resulturl = new Uri(
                    resulturl
                    ?? Environment.GetEnvironmentVariable("TRXPARSER_RESULTURL")
                    ?? throw new Exception("\n=>URL to the test result page is not set!\n")
                );

                var _repourl = new Uri(
                    repourl
                    ?? Environment.GetEnvironmentVariable("TRXPARSER_REPOURL")
                    ?? throw new Exception("\n=>URL to code repo not set!\n")
                );

                var _webhook = new Uri(
                    webhook
                    ?? Environment.GetEnvironmentVariable("TRXPARSER_WEBHOOK")
                    ?? throw new Exception("\n=>URL to the WebHook not set!\n")
                );

                var _searchpath = searchpath
                    ?? Environment.GetEnvironmentVariable("TRXPARSER_SEARCHPATH")
                    ?? throw new Exception("\n=>Search path for TRX files not set!\n");
                if (!Directory.Exists(_searchpath))
                {
                    throw new Exception($"\n=>TRX path {_searchpath} not exists on this machine!\n");
                }

                var _deltrx = deltrx
                    || bool.Parse(
                        Environment.GetEnvironmentVariable("TRXPARSER_DELTRX")
                        ?? "false");

                var _debug = debug
                    || bool.Parse(
                        Environment.GetEnvironmentVariable("TRXPARSER_DEBUG")
                        ?? "false");
            #endregion

            if (_debug)
            {
                Console.WriteLine(_title);
                Console.WriteLine(_repourl);
                Console.WriteLine(_resulturl);
                Console.WriteLine(_webhook);
                Console.WriteLine(_searchpath);
                Console.WriteLine(_deltrx);
                Console.WriteLine(_oklimit);
            }

            var byteArray = Resources.MessageBody;
            var _messageBody = new StringBuilder(
                Encoding.UTF8.GetString(byteArray, 3, byteArray.Length - 3));

            #region Prepare table
                ResultTable = new DataTable();
                ResultTable.Columns.Add("group", typeof(string));
                ResultTable.Columns.Add("testName", typeof(string));
                ResultTable.Columns.Add("outcome", typeof(string));
            #endregion

            #region crawl all TRX files inside _searchpath
                string[] fileList = Directory.GetFiles(_searchpath, "*.trx", 
                    SearchOption.AllDirectories);
                if (_debug) 
                {
                    Console.WriteLine($"{fileList.Length} TRX files found in {_searchpath}");
                }
                foreach (var file in fileList)
                {
                    GetTestData(file);
                }
            #endregion

            #region Generate result Markdown
                var Markdown = new StringBuilder();
                foreach (DataRow row in ResultTable.Select("outcome <> 'Passed'"))
                {
                    IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                    Markdown.Append($"- {string.Join(", ", fields).Replace("\"", "´")} \n\r");
                }
                var errorListMarkdownString = Markdown.ToString();
            #endregion

            #region Calculate conclusion
                var total = (double)ResultTable.Rows.Count;
                var passed = (double)ResultTable.Select("outcome = 'Passed'").Length;
                var failed = total - passed;
                var percentage = Math.Round(((passed / total) * 100), 2);
            #endregion

            #region Parse WebHooks Message Body
                _messageBody.Replace("{date}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                _messageBody.Replace("{title}", _title);
                _messageBody.Replace("{total}", total.ToString());
                _messageBody.Replace("{passed}", passed.ToString());
                _messageBody.Replace("{failed}", failed.ToString());
                _messageBody.Replace("{repourl}", _repourl.ToString());
                _messageBody.Replace("{resulturl}", _resulturl.ToString());

                if (percentage < _oklimit)
                {
                    _messageBody.Replace("{status}", 
                        "Testset **fails**");
                    _messageBody.Replace("{subtitle}", 
                        $"Testset **fails** with **{percentage}%** passed test");
                    _messageBody.Replace("{icon}", NOK);
                }
                else
                {
                    _messageBody.Replace("{status}", 
                        "Testset **passed**");
                    _messageBody.Replace("{subtitle}", 
                        $"Testset **passed** with **{percentage}%** passed test");
                    _messageBody.Replace("{icon}", OK);
                }

                if (failed == 0)
                {
                    _messageBody.Replace("{activity}", "");
                }
                else
                {
                    _messageBody.Replace("{activity}", 
                        $"Please check the following tests:\n\r{errorListMarkdownString}");
                }
            #endregion

            if (_debug)
            {
                Console.WriteLine(_messageBody);
            }

            #region WebHooks Message senden
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage
                    {
                        RequestUri = _webhook,
                        Method = HttpMethod.Post,
                    };
                    request.Content = new ByteArrayContent(
                        Encoding.UTF8.GetBytes(_messageBody.ToString()));
                    request.Content.Headers.ContentType = 
                        new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage result = client.SendAsync(request).Result;
                    result.EnsureSuccessStatusCode();
                }
            #endregion

            #region Delete all TRX files when all is done and _deltrx is set
                if (_deltrx)
                {
                    foreach (var file in Directory.GetFiles(
                        _searchpath, "*.trx", SearchOption.AllDirectories))
                    {
                            File.Delete(file);
                    }
                }
            #endregion

            return 0;
        }

        public static void GetTestData(string file)
        {
            var fileName = file.Split('\\')[file.Split('\\').Length - 1].Replace(".trx", "");
            var xelement = XElement.Load(file);
            var res = from result in xelement.Descendants()
                      where result.Name.LocalName == "UnitTestResult"
                      select new
                      {
                          outcome = result.Attribute("outcome").Value,
                          testName = result.Attribute("testName").Value
                      };

            foreach (var result in res)
            {
                DataRow newRow = ResultTable.NewRow();
                newRow["group"] = fileName;
                newRow["outcome"] = result.outcome;
                newRow["testName"] = result.testName;
                ResultTable.Rows.Add(newRow);
                Console.WriteLine($"{fileName} - {result.outcome} - {result.testName}");
            }
        }
    }
}

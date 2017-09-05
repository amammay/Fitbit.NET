﻿using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Fitbit.Api.Portable;
using Fitbit.Api.Portable.Models;
using FluentAssertions;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class SleepTests
    {
    
        [Test]
        [Category("Portable")]
        public async void GetSleepAsyncOld_Success()
        {
            string content = SampleDataHelper.GetContent("GetSleepOld.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/sleep/date/2014-10-17.json",
                    message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            var response = await fitbitClient.GetSleepAsync(new DateTime(2014, 10, 17));

            ValidateSleepOld(response);
        }

        [Test]
        [Category("Portable")]
        public async void GetSleepAsync_Success()
        {
            string content = SampleDataHelper.GetContent("GetSleep.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) {Content = new StringContent(content)};
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1.2/user/-/sleep/date/2014-10-17.json",
                    message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            var response = await fitbitClient.GetSleepDateAsync(new DateTime(2014, 10, 17));

            ValidateSleep(response);
        }

        [Test]
        [Category("Portable")]
        public async void GetSleepRangeAsync_Success()
        {
            string content = SampleDataHelper.GetContent("GetSleepRange.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1.2/user/-/sleep/date/2017-04-02/2017-04-02.json",
                    message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            var response = await fitbitClient.GetSleepDateRangeAsync(new DateTime(2017, 04, 02), new DateTime(2017, 04, 02));

            ValidateSleepRange(response);
        }

        [Test]
        [Category("Portable")]
        public async void GetSleepLogListAsync_Success()
        {
            string content = SampleDataHelper.GetContent("SleepLogList.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1.2/user/-/sleep/list.json?beforeDate=2017-03-27&sort=desc&offset=0&limit=1",
                    message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            var response = await fitbitClient.GetSleepLogListAsync(new DateTime(2017,3,27), SleepEnum.Before, SortEnum.Desc, 1 );

            ValidateSleepLogList(response);
        }

        [Test]
        [Category("Portable")]
        public void GetSleepAsyncOld_Failure_Errors()
        {
            var responseMessage = Helper.CreateErrorResponse();
            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            Func<Task<SleepData>> result = () => fitbitClient.GetSleepAsync(new DateTime(2014, 11, 11));

            result.ShouldThrow<FitbitException>();
        }

        [Test]
        [Category("Portable")]
        public void GetSleepAsync_Failure_Errors()
        {
            var responseMessage = Helper.CreateErrorResponse();
            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            Func<Task<SleepLogDateBase>> result = () => fitbitClient.GetSleepDateAsync(new DateTime(2017, 4, 4));

            result.ShouldThrow<FitbitException>();
        }

        [Test]
        [Category("Portable")]
        public void GetSleepRangeAsync_Failure_Errors()
        {
            var responseMessage = Helper.CreateErrorResponse();
            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            Func<Task<SleepDateRangeBase>> result = () => fitbitClient.GetSleepDateRangeAsync(new DateTime(2017, 4, 4), new DateTime(2017,6,23));

            result.ShouldThrow<FitbitException>();
        }

        [Test]
        [Category("Portable")]
        public void GetSleepLogListAsync_Failure_Errors()
        {
            var responseMessage = Helper.CreateErrorResponse();
            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            Func<Task<SleepLogListBase>> result = () => fitbitClient.GetSleepLogListAsync(new DateTime(2017,4,4), SleepEnum.After, SortEnum.Asc, 2 );

            result.ShouldThrow<FitbitException>();
        }
        
        [Test]
        [Category("Portable")]
        public void PostLogSleepAsync_Failure_Errors()
        {
            var responseMessage = Helper.CreateErrorResponse();
            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Post, message.Method);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            Func<Task<SleepLogDateRange>> result = () => fitbitClient.PostLogSleepAsync("12:12",1,new DateTime(2017,4,4));

            result.ShouldThrow<FitbitException>();
        }

        [Test]
        [Category("Portable")]
        public void Can_Deserialize_Sleep_Old()
        {
            string content = SampleDataHelper.GetContent("GetSleepOld.json");
            var deserializer = new JsonDotNetSerializer();

            SleepData sleep = deserializer.Deserialize<SleepData>(content);

            ValidateSleepOld(sleep);
        }

        [Test]
        [Category("Portable")]
        public void Can_Deserialize_Sleep()
        {
            string content = SampleDataHelper.GetContent("GetSleep.json");
            var deserializer = new JsonDotNetSerializer();

            SleepLogDateBase sleep = deserializer.Deserialize<SleepLogDateBase>(content);

            ValidateSleep(sleep);
        }

        [Test]
        [Category("Portable")]
        public void Can_Deserialize_Sleep_Range()
        {
            string content = SampleDataHelper.GetContent("GetSleepRange.json");
            var deserializer = new JsonDotNetSerializer();

            SleepDateRangeBase sleep = deserializer.Deserialize<SleepDateRangeBase>(content);

            ValidateSleepRange(sleep);
        }

        [Test]
        [Category("Portable")]
        public void Can_Deserialize_Sleep_Log_List()
        {
            string content = SampleDataHelper.GetContent("SleepLogList.json");
            var deserializer = new JsonDotNetSerializer();

            SleepLogListBase sleep = deserializer.Deserialize<SleepLogListBase>(content);

            ValidateSleepLogList(sleep);
        }

        private void ValidateSleepRange(SleepDateRangeBase sleep)
        {
            // general
            Assert.IsNotNull(sleep);
            Assert.IsNotNull(sleep.Sleep);

            // logs
            Assert.AreEqual(1, sleep.Sleep.Length);
            var first = sleep.Sleep.First();
            Assert.AreEqual(new DateTime(2017, 4, 2).Date, first.DateOfSleep);
            Assert.AreEqual(28800000, first.Duration);
            Assert.AreEqual(85, first.Efficiency);
            Assert.AreEqual(true, first.IsMainSleep);
            Assert.AreEqual(15040942043, first.LogId);
            Assert.AreEqual(0, first.MinutesAfterWakeup);

            Assert.AreEqual(500, first.MinutesAsleep);
            Assert.AreEqual(86, first.MinutesAwake);
            Assert.AreEqual(0, first.MinutesToFallAsleep);
            Assert.AreEqual(new DateTime(2017, 4, 1), first.StartTime);
            Assert.AreEqual(586, first.TimeInBed);
            Assert.AreEqual("stages", first.Type);

            //sleep log levels
            var levels = first.Levels;
            Assert.IsNotNull(levels.Summary);
            Assert.IsNotNull(levels.Data);
            Assert.IsNotNull(levels.ShortData);

            // Levels Summary deep
            Assert.AreEqual(5, levels.Summary.Deep.Count);
            Assert.AreEqual(99, levels.Summary.Deep.Minutes);
            Assert.AreEqual(70, levels.Summary.Deep.ThirtyDayAvgMinutes);

            // Levels Summary light
            Assert.AreEqual(32, levels.Summary.Light.Count);
            Assert.AreEqual(296, levels.Summary.Light.Minutes);
            Assert.AreEqual(247, levels.Summary.Light.ThirtyDayAvgMinutes);

            // Levels Summary rem
            Assert.AreEqual(12, levels.Summary.Rem.Count);
            Assert.AreEqual(105, levels.Summary.Rem.Minutes);
            Assert.AreEqual(81, levels.Summary.Rem.ThirtyDayAvgMinutes);

            // Levels Summary wake
            Assert.AreEqual(39, levels.Summary.Wake.Count);
            Assert.AreEqual(86, levels.Summary.Wake.Minutes);
            Assert.AreEqual(72, levels.Summary.Wake.ThirtyDayAvgMinutes);

            //sleep log data
            var data = levels.Data.First();
            Assert.IsNotNull(data);
            Assert.AreEqual(new DateTime(2017, 4, 1), data.DateTime);
            Assert.AreEqual("wake", data.Level);
            Assert.AreEqual(360, data.Seconds);

            //sleep log short data
            var dataShort = levels.ShortData.First();
            Assert.IsNotNull(dataShort);
            Assert.AreEqual(new DateTime(2017, 4, 2), dataShort.DateTime);
            Assert.AreEqual("wake", dataShort.Level);
            Assert.AreEqual(560, dataShort.Seconds);
        }

        private void ValidateSleep(SleepLogDateBase sleep)
        {
            // general
            Assert.IsNotNull(sleep);
            Assert.IsNotNull(sleep.Summary);
            Assert.IsNotNull(sleep.Sleep);

            // summary
            var summary = sleep.Summary;
            Assert.AreEqual(500, summary.TotalMinutesAsleep);
            Assert.AreEqual(1, summary.TotalSleepRecords);
            Assert.AreEqual(586, summary.TotalTimeInBed);

            // logs
            Assert.AreEqual(1, sleep.Sleep.Length);
            var first = sleep.Sleep.First();
            Assert.AreEqual(new DateTime(2017, 4, 2).Date, first.DateOfSleep);
            Assert.AreEqual(28800000, first.Duration);
            Assert.AreEqual(85, first.Efficiency);
            Assert.AreEqual(true, first.IsMainSleep);
            Assert.AreEqual(15040942043, first.LogId);
            Assert.AreEqual(0, first.MinutesAfterWakeup);

            Assert.AreEqual(500, first.MinutesAsleep);
            Assert.AreEqual(86, first.MinutesAwake);
            Assert.AreEqual(0, first.MinutesToFallAsleep);
            Assert.AreEqual(new DateTime(2017, 4, 1), first.StartTime);
            Assert.AreEqual(586, first.TimeInBed);
            Assert.AreEqual("stages", first.Type);

            //sleep log levels
            var levels = first.Levels;
            Assert.IsNotNull(levels.Summary);
            Assert.IsNotNull(levels.Data);
            Assert.IsNotNull(levels.ShortData);

            // Levels Summary deep
            Assert.AreEqual(5, levels.Summary.Deep.Count);
            Assert.AreEqual(99, levels.Summary.Deep.Minutes);
            Assert.AreEqual(70, levels.Summary.Deep.ThirtyDayAvgMinutes);

            // Levels Summary light
            Assert.AreEqual(32, levels.Summary.Light.Count);
            Assert.AreEqual(296, levels.Summary.Light.Minutes);
            Assert.AreEqual(247, levels.Summary.Light.ThirtyDayAvgMinutes);

            // Levels Summary rem
            Assert.AreEqual(12, levels.Summary.Rem.Count);
            Assert.AreEqual(105, levels.Summary.Rem.Minutes);
            Assert.AreEqual(81, levels.Summary.Rem.ThirtyDayAvgMinutes);

            // Levels Summary wake
            Assert.AreEqual(39, levels.Summary.Wake.Count);
            Assert.AreEqual(86, levels.Summary.Wake.Minutes);
            Assert.AreEqual(72, levels.Summary.Wake.ThirtyDayAvgMinutes);

            //sleep log data
            var data = levels.Data.First();
            Assert.IsNotNull(data);
            Assert.AreEqual(new DateTime(2017, 4, 1), data.DateTime);
            Assert.AreEqual("wake", data.Level);
            Assert.AreEqual(360, data.Seconds);

            //sleep log short data
            var dataShort = levels.ShortData.First();
            Assert.IsNotNull(dataShort);
            Assert.AreEqual(new DateTime(2017, 4, 2), dataShort.DateTime);
            Assert.AreEqual("wake", dataShort.Level);
            Assert.AreEqual(560, dataShort.Seconds);
        }

        private void ValidateSleepOld(SleepData sleep)
        {
            // general
            Assert.IsNotNull(sleep);
            Assert.IsNotNull(sleep.Summary);
            Assert.IsNotNull(sleep.Sleep);

            // summary
            var summary = sleep.Summary;
            Assert.AreEqual(518, summary.TotalMinutesAsleep);
            Assert.AreEqual(2, summary.TotalSleepRecords);
            Assert.AreEqual(540, summary.TotalTimeInBed);

            // logs
            Assert.AreEqual(2, sleep.Sleep.Count);
            var l = sleep.Sleep.First();

            Assert.AreEqual(true, l.IsMainSleep);
            Assert.AreEqual(29744, l.LogId);
            Assert.AreEqual(98, l.Efficiency);
            Assert.AreEqual(new DateTime(2011, 6, 16), l.StartTime);
            Assert.AreEqual(28800000, l.Duration);
            Assert.AreEqual(0, l.MinutesToFallAsleep);
            Assert.AreEqual(480, l.MinutesAsleep);
            Assert.AreEqual(0, l.MinutesAwake);
            Assert.AreEqual(0, l.MinutesAfterWakeup);
            // awakenings count - depcrecated
            Assert.AreEqual(0, l.AwakeCount);
            Assert.AreEqual(0, l.AwakeDuration);
            Assert.AreEqual(0, l.RestlessCount);
            Assert.AreEqual(0, l.RestlessDuration);
            Assert.AreEqual(480, l.TimeInBed);
            Assert.AreEqual(3, l.MinuteData.Count);

            var min = l.MinuteData.First();
            Assert.IsNotNull(min);
            Assert.AreEqual(new DateTime(1900, 1, 1).TimeOfDay, min.DateTime.TimeOfDay);
            Assert.AreEqual(3, min.Value);
            l.MinuteData.Remove(min);

            min = l.MinuteData.First();
            Assert.IsNotNull(min);
            Assert.AreEqual(new DateTime(1900, 1, 1, 0, 1, 0).TimeOfDay, min.DateTime.TimeOfDay);
            Assert.AreEqual(2, min.Value);
            l.MinuteData.Remove(min);

            min = l.MinuteData.First();
            Assert.IsNotNull(min);
            Assert.AreEqual(new DateTime(1900, 1, 1, 0, 2, 0).TimeOfDay, min.DateTime.TimeOfDay);
            Assert.AreEqual(1, min.Value);

            sleep.Sleep.Remove(l);
            l = sleep.Sleep.First();

            Assert.AreEqual(false, l.IsMainSleep);
            Assert.AreEqual(29745, l.LogId);
            Assert.AreEqual(93, l.Efficiency);
            Assert.AreEqual(new DateTime(2011, 6, 16, 14, 0, 0), l.StartTime);
            Assert.AreEqual(3600000, l.Duration);
            Assert.AreEqual(20, l.MinutesToFallAsleep);
            Assert.AreEqual(38, l.MinutesAsleep);
            Assert.AreEqual(0, l.MinutesAwake);
            Assert.AreEqual(2, l.MinutesAfterWakeup);
            // awakenings count - depcrecated
            Assert.AreEqual(0, l.AwakeCount);
            Assert.AreEqual(0, l.AwakeDuration);
            Assert.AreEqual(0, l.RestlessCount);
            Assert.AreEqual(0, l.RestlessDuration);
            Assert.AreEqual(60, l.TimeInBed);
            Assert.AreEqual(1, l.MinuteData.Count);

            min = l.MinuteData.First();
            Assert.IsNotNull(min);
            Assert.AreEqual(new DateTime(1900, 1, 1, 14, 0, 0).TimeOfDay, min.DateTime.TimeOfDay);
            Assert.AreEqual(3, min.Value);
        }

        private void ValidateSleepLogList(SleepLogListBase sleep)
        {
            //General
            Assert.IsNotNull(sleep);
            Assert.IsNotNull(sleep.Pagination);
            Assert.IsNotNull(sleep.Sleep);

            //Pagination 
            var sleepPagination = sleep.Pagination;
            Assert.AreEqual(new DateTime(2017,03,27), sleepPagination.BeforeDate);
            Assert.AreEqual(1, sleepPagination.Limit);
            Assert.AreEqual("https://api.fitbit.com/1.2/user/-/sleep/list.json?offset=1&limit=1&sort=desc&beforeDate=2017-03-27", sleepPagination.Next);
            Assert.IsEmpty(sleepPagination.Previous);
            Assert.AreEqual(0, sleepPagination.Offset);
            Assert.AreEqual(SortEnum.Desc.ToString(), sleepPagination.Sort);

            var firstSleep = sleep.Sleep.First();

            //sleep log levels
            var levels = firstSleep.Levels;
            Assert.IsNotNull(levels.Summary);
            Assert.IsNotNull(levels.Data);
            Assert.IsNotNull(levels.ShortData);

            // Levels Summary deep
            Assert.AreEqual(0, levels.Summary.Deep.Count);
            Assert.AreEqual(0, levels.Summary.Deep.Minutes);
            Assert.AreEqual(0, levels.Summary.Deep.ThirtyDayAvgMinutes);

            // Levels Summary light
            Assert.AreEqual(0, levels.Summary.Light.Count);
            Assert.AreEqual(0, levels.Summary.Light.Minutes);
            Assert.AreEqual(0, levels.Summary.Light.ThirtyDayAvgMinutes);

            // Levels Summary rem
            Assert.AreEqual(0, levels.Summary.Rem.Count);
            Assert.AreEqual(0, levels.Summary.Rem.Minutes);
            Assert.AreEqual(0, levels.Summary.Rem.ThirtyDayAvgMinutes);

            // Levels Summary wake
            Assert.AreEqual(0, levels.Summary.Wake.Count);
            Assert.AreEqual(0, levels.Summary.Wake.Minutes);
            Assert.AreEqual(0, levels.Summary.Wake.ThirtyDayAvgMinutes);

            //sleep log data
            var data = levels.Data.First();
            Assert.IsNotNull(data);
            Assert.AreEqual(new DateTime(2017, 3, 25, 23,58,30), data.DateTime);
            Assert.AreEqual("wake", data.Level);
            Assert.AreEqual(0, data.Seconds);

            //sleep log short data
            var dataShort = levels.ShortData.First();
            Assert.IsNotNull(dataShort);
            Assert.AreEqual(new DateTime(2017, 3, 26,5,58,30), dataShort.DateTime);
            Assert.AreEqual("wake", dataShort.Level);
            Assert.AreEqual(0, dataShort.Seconds);

            Assert.AreEqual(0, firstSleep.LogId);
            Assert.AreEqual(0, firstSleep.MinutesAfterWakeup);
            Assert.AreEqual(0, firstSleep.MinutesAsleep);
            Assert.AreEqual(0, firstSleep.MinutesAwake);
            Assert.AreEqual(0, firstSleep.MinutesToFallAsleep);
            Assert.AreEqual(new DateTime(2017, 03, 25,23,58,30), firstSleep.StartTime);
            Assert.AreEqual(0, firstSleep.TimeInBed);
            Assert.AreEqual("stages", firstSleep.Type);

        }

    }
}
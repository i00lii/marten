﻿using System;
using System.Linq;
using Baseline;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Marten.Testing.Bugs
{
    public class Bug_432_querying_with_UTC_times_with_offset : IntegratedFixture
    {
        private readonly ITestOutputHelper _output;

        public Bug_432_querying_with_UTC_times_with_offset(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void can_issue_queries_against_the_offset()
        {


            using (var session = theStore.LightweightSession())
            {
                var now = DateTime.UtcNow.ToUniversalTime();
                _output.WriteLine("now: " + now.ToString("F"));
                var testClass = new DateClass
                {
                    Id = Guid.NewGuid(),
                    DateTimeField = now
                };

                session.Store(testClass);

                session.Store(new DateClass
                {
                    DateTimeField = now.Add(5.Minutes())
                });

                session.Store(new DateClass
                {
                    DateTimeField = now.Add(-5.Minutes())
                });

                session.SaveChanges();

                var cmd = session.Query<DateClass>().Where(x => now >= x.DateTimeField)
                    .ToCommand();

                _output.WriteLine(cmd.CommandText);


                session.Query<DateClass>().ToList().Each(x =>
                {
                    _output.WriteLine(x.DateTimeField.ToString(""));
                });

                session.Query<DateClass>()
                    .Count(x => now >= x.DateTimeField).ShouldBe(2);

            }
        }

        [Fact]
        public void can_issue_queries_against_the_offset_as_duplicated_column()
        {
            StoreOptions(_ => _.Schema.For<DateClass>().Duplicate(x => x.DateTimeField));

            using (var session = theStore.LightweightSession())
            {
                var now = DateTime.Now;
                _output.WriteLine("now: " + now.ToString("o"));
                var testClass = new DateClass
                {
                    Id = Guid.NewGuid(),
                    DateTimeField = now
                };

                session.Store(testClass);

                session.Store(new DateClass
                {
                    DateTimeField = now.Add(5.Minutes())
                });

                session.Store(new DateClass
                {
                    DateTimeField = now.Add(-5.Minutes())
                });

                session.SaveChanges();

                var cmd = session.Query<DateClass>().Where(x => now >= x.DateTimeField)
                    .ToCommand();

                _output.WriteLine(cmd.CommandText);


                session.Query<DateClass>().ToList().Each(x =>
                {
                    _output.WriteLine(x.DateTimeField.ToString("o"));
                });

                session.Query<DateClass>()
                    .Count(x => now >= x.DateTimeField).ShouldBe(2);

            }
        }

        [Fact]
        public void can_issue_queries_against_the_datetime_offset()
        {
            using (var session = theStore.LightweightSession())
            {
                var now = DateTime.UtcNow;
                _output.WriteLine("now: " + now.ToString("o"));
                var testClass = new DateOffsetClass()
                {
                    Id = Guid.NewGuid(),
                    DateTimeField = now
                };

                session.Store(testClass);

                session.Store(new DateOffsetClass()
                {
                    DateTimeField = now.Add(5.Minutes())
                });

                session.Store(new DateOffsetClass()
                {
                    DateTimeField = now.Add(-5.Minutes())
                });

                session.SaveChanges();

                var cmd = session.Query<DateOffsetClass>().Where(x => now >= x.DateTimeField)
                    .ToCommand();

                _output.WriteLine(cmd.CommandText);


                session.Query<DateOffsetClass>().ToList().Each(x =>
                {
                    _output.WriteLine(x.DateTimeField.ToString("o"));
                });

                session.Query<DateOffsetClass>()
                    .Count(x => now >= x.DateTimeField).ShouldBe(2);

            }
        }

        [Fact]
        public void can_issue_queries_against_the_datetime_offset_as_duplicate_field()
        {
            StoreOptions(_ => _.Schema.For<DateOffsetClass>().Duplicate(x => x.DateTimeField));

            using (var session = theStore.LightweightSession())
            {
                var now = DateTimeOffset.UtcNow;
                _output.WriteLine("now: " + now.ToString("o"));
                var testClass = new DateOffsetClass()
                {
                    Id = Guid.NewGuid(),
                    DateTimeField = now
                };

                session.Store(testClass);

                session.Store(new DateOffsetClass()
                {
                    DateTimeField = now.Add(5.Minutes())
                });

                session.Store(new DateOffsetClass()
                {
                    DateTimeField = now.Add(-5.Minutes())
                });

                session.SaveChanges();

                var cmd = session.Query<DateOffsetClass>().Where(x => now >= x.DateTimeField)
                    .ToCommand();

                _output.WriteLine(cmd.CommandText);


                session.Query<DateOffsetClass>().ToList().Each(x =>
                {
                    _output.WriteLine(x.DateTimeField.ToString("o"));
                });

                session.Query<DateOffsetClass>()
                    .Count(x => now >= x.DateTimeField).ShouldBe(2);

            }
        }


    }

    public class DateClass
    {
        public Guid Id { get; set; }
        public DateTime DateTimeField { get; set; }
    }

    public class DateOffsetClass
    {
        public Guid Id { get; set; }
        public DateTimeOffset DateTimeField { get; set; }
    }
}

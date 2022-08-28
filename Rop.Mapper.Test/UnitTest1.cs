using Rop.Mapper.Test.Model;

namespace Rop.Mapper.Test
{
    public class UnitTest1
    {
        [Fact]
        public void TestNormal()
        {
            var (origin, expected) = MakeClasses.MakeNormal();
            var destiny=origin.MapTo<DestinyNormal>();
            Assert.Equal(expected,destiny);
        }
        [Fact]
        public void TestError()
        {
            var (origin, expected) = MakeClasses.MakeError();
            var ex=Assert.Throws<Exception>(() => origin.MapTo<DestinyError>());
            Assert.NotNull(ex);
        }
        [Fact]
        public void TestVerify()
        {
            var (originok, expectedok) = MakeClasses.MakeNormal();
            var (origin, expected) = MakeClasses.MakeError();
            var ok1 = Mapper.Verify(originok.GetType(), expectedok.GetType());
            var ok2 = Mapper.Verify(origin.GetType(), expected.GetType());
            Assert.True(ok1);
            Assert.False(ok2);
        }
        [Fact]
        public void TestIgnore()
        {
            var (origin, expected) = MakeClasses.MakeIgnore1();
            var ok1 = Mapper.Verify(origin.GetType(), expected.GetType());
            Assert.True(ok1);
            var destiny = origin.MapTo<DestinyIgnore1>();
            Assert.Equal(expected, destiny);
        }
        [Fact]
        public void TestIgnore2()
        {
            var (origin, expected1,expected2) = MakeClasses.MakeIgnore2();
            var ok1 = Mapper.Verify(origin.GetType(), expected1.GetType());
            var ok2 = Mapper.Verify(origin.GetType(), expected2.GetType());
            Assert.True(ok1);
            Assert.False(ok2);
            var destiny1 = origin.MapTo<DestinyIgnore2A>();
            var ex = Assert.Throws<Exception>(() => origin.MapTo<DestinyIgnore2B>());
            Assert.Equal(expected1.BirthDate, destiny1.BirthDate);
            Assert.Equal(expected1.Name, destiny1.Name);
            Assert.Equal(expected1.Surname, destiny1.Surname);
            Assert.Equal(expected1.UpdateDate, destiny1.UpdateDate);
            var e = expected1.Equals(destiny1);
            Assert.Equal(expected1,destiny1);
        }

        [Fact]
        public void TestMapsTo()
        {
            var (origin, expected) = MakeClasses.MakeMapsTo();
            var destiny = origin.MapTo<DestinyMapsTo>();
            Assert.Equal(expected, destiny);
        }
        [Fact]
        public void TestMapsFrom()
        {
            var (origin, expected) = MakeClasses.MakeMapsFrom();
            var destiny = origin.MapTo<DestinyMapsFrom>();
            Assert.Equal(expected, destiny);
        }
        [Fact]
        public void TestUseNull()
        {
            var (origin, expected) = MakeClasses.MakeUseNullValue();
            var destiny = origin.MapTo<DestinyUseNull>();
            Assert.Equal(expected, destiny);
        }
        [Fact]
        public void TestStringToList()
        {
            var (origin, expected1,expected2) = MakeClasses.MakeUseStringToList();
            var destiny1 = origin.MapTo<DestinyStringToList>();
            var destiny2 = origin.MapTo<DestinyStringToArray>();
            Assert.Equal(expected1.Groups, destiny1.Groups);
            Assert.Equal(expected2.Groups, destiny2.Groups);

        }
    }
}
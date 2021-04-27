using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace ROP.UnitTest.Serializer
{
    public class TestResultDtoOnSerialization
    {
        [Fact]
        public void Test()
        {
            ResultDto<PlaceHolder> obj = new ResultDto<PlaceHolder>(new PlaceHolder(1));

            string json = JsonSerializer.Serialize(obj);

            Assert.NotEmpty(json);

            var resultDto = JsonSerializer.Deserialize<ResultDto<PlaceHolder>>(json);
            Assert.Equal(obj.Value.Id, resultDto.Value.Id);

        }


        public class PlaceHolder
        {
            public int Id { get; private set; }

            public PlaceHolder(int id)
            {
                Id = id;
            }
        }
    }
}

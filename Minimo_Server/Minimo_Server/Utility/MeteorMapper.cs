using MinimoServer.Models;

namespace MinimoServer.Utility;
using MinimoShared;

public class MeteorMapper
{
    public static MeteorDTO ToMeteorDTO(Meteor meteor)
    {
        return new MeteorDTO
        {
            Id = meteor.Id,
            MeteorType = meteor.MeteorType,
            ValueIndex = meteor.ValueIndex,
            ValueCount = meteor.ValueCount,
        };
    }
    
    public static Meteor ToMeteor(MeteorDTO meteorDto)
    {
        return new Meteor
        {
            Id = meteorDto.Id,
            MeteorType = meteorDto.MeteorType,
            ValueIndex = meteorDto.ValueIndex,
            ValueCount = meteorDto.ValueCount,
        };
    }
}
using EventConnect.Data.BaseEntity;

namespace EventConnect.Entities.Security;

public class PasswordValues:BaseEntity
{
    public byte[] PasswordHash { get; set; } = new byte[0];
    public byte[] PasswordSalt{ get; set; } = new byte[0];
}
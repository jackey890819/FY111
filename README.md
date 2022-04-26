# FY111 ASP.NET 後端程式
- Framework：   .net core 3.1
- Database：    MySQL (MariaDB)

## 產生 ASP.NET User 資料表

1. 先自行創建一個名為"FY111User"的資料庫

2. 到套件管理器控制台打以下指令

    1. `Add-Migration "InitialCreate" -Context "FY111UserDbContext"`

    2. `Update-Database -c "FY111UserDbContext"`

3. 在 **aspnetroles** 資料表中加上以下資料

    Id|Name|NormalizedName|ConcurrencyStamp
    --|----|--------------|----------------
    1|SuperAdmin|SUPERADMIN|NULL
    2|MetaverseAdmin|METAVERSEADMIN|NULL
    3|GroupUser|GROUPUSER|NULL
    4|NormalUser|NORMALUSER|NULL


# FY111 ASP.NET 後端程式
- Framework：   .net core 3.1
- Database：    MySQL (MariaDB)

## MySQL Database

- 專案預設帳號root，密碼root

- 使用資料庫

    1. FY111

    2. FY111User

## 產生 FY111 資料庫

1. 使用SQL資料夾中的FY111.sql

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

4. 使用register API 或是網頁產生 User

5. 進到資料庫裡更改權限

123

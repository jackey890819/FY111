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

1. 使用SQL資料夾中的fy111user.sql

2. 使用者帳密、權限、關係詳情

    依序為帳號、密碼、使用者權限

    User01  User01@ SuperAdmin

    User02  User02@ ClassAdmin

    User03  User03@ GroupUser

    User04  User04@ NormalUser

    User05  User05@ NormalUser

    User06  User06@ NormalUser

    User05和User06為Group FY111的成員


## 
## 部屬 ASP .NET Core 至 IIS (區網測試用)
1. 安裝 Windows Hosting Bundle

    https://dotnet.microsoft.com/zh-cn/download/dotnet/3.1
    
    下載對應版本的hosting bundle
    
2. 開啟IIS

    控制台>程式集>開啟或關閉Windows功能

	勾選Internet Information Services 中的 web管理工具 及 world wide web服務

3. 新增站台

    打開IIS管理員>左邊站台新增站台

	輸入站台名稱、實體路徑(到時候要發佈到這個位置)

	繫結依需求輸入，連接埠選輸入沒被占用的 ex:81

4. 發布專案

    VS中>建置>發佈

	目標選資料夾>資料夾位置為站台的實體路徑

	建好設定檔後按發佈

	等它跑完...

至此已可於本地進入剛所發佈的網站

5. 新增防火牆輸入規則

	打開具進階安全性的Windows Defender 防火牆

	左邊點選輸入規則後 右邊新增規則

	規則類型:連接埠

    通訊協定及連接埠:TCP, 輸入剛設定的連接埠 (步驟3中的)

	動作:允許連線 (測試用就不改安全連線)

	設定檔:三個都勾

	名稱：名稱跟描述可以大致寫一下，好被認出

	完成後確認"已啟用"為是

結束，用其他設備測試。

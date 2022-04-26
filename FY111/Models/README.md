# Models

## 將資料表轉成 Models

- 在套件管理器主控台打指令

    ```
    Scaffold-DbContext "Server=localhost; Port=3306;User Id=root;Password=root;Database=FY111;" MySql.EntityFrameworkCore -OutputDir Models/FY111 -f
    ```
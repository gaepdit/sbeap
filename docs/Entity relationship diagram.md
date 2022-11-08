# Entity relationship diagram

```mermaid
erDiagram

Customer ||--o{ Case        : has
Case     ||--o{ Action-Item : contains
Customer ||--o{ Contact     : has
```

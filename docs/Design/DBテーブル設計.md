# DBテーブル設計

Issue #19 にて確定した決定版。EF Core + PostgreSQL を使用する。

## 設計方針

* **固定長の関係は列で持つ**（種族タイプ×2、特性×3、技×4、パーティスロット×6）
* **タイプ相性はコード管理**（TypeChart）のためDBテーブル不要
* **能力補正の詳細はコード管理**。StatAlignmentsテーブルは名前解決用
* **能力ポイントバリデーションはアプリ側**で実施（DB制約ではない）
* **ID体系**: マスターデータ系は `int`（PokeAPI ID）、ユーザーデータ系は `text`（prefix\_uuidv7）
* **タイムスタンプ**: 全テーブルに `CreatedAt` / `UpdatedAt`（`timestamp with time zone`、デフォルト `CURRENT_TIMESTAMP`）を付与。`UpdatedAt` は `SaveChangesInterceptor` で自動更新

## マスターデータテーブル

PokeAPIから取得・変換して保存する。

### Types（ポケモンのタイプ）

| 列名 | 型 | 制約 | メモ |
| ----- | ----- | ----- | ----- |
| TypeId | int | PK | タイプID PokeAPIでのid |
| TypeName | text | NOT NULL | タイプ名 日本語名 |
| CreatedAt | timestamp with time zone | NOT NULL, DEFAULT | 作成日時 |
| UpdatedAt | timestamp with time zone | NOT NULL, DEFAULT | 更新日時 |

### Species（ポケモンの種族）

| 列名 | 型 | 制約 | メモ |
| ----- | ----- | ----- | ----- |
| SpeciesId | int | PK | 種族ID PokeAPIでのpokemonエンドポイントのid |
| SpeciesName | text | NOT NULL | 種族名 日本語名 |
| Type1Id | int | FK, NOT NULL | タイプ1ID |
| Type2Id | int | FK | タイプ2ID |
| Ability1Id | int | FK, NOT NULL | 特性1ID |
| Ability2Id | int | FK | 特性2ID |
| HiddenAbilityId | int | FK | 隠れ特性ID |
| BaseStatHp | int | NOT NULL | 種族値::HP |
| BaseStatAttack | int | NOT NULL | 種族値::こうげき |
| BaseStatDefense | int | NOT NULL | 種族値::ぼうぎょ |
| BaseStatSpecialAttack | int | NOT NULL | 種族値::とくこう |
| BaseStatSpecialDefense | int | NOT NULL | 種族値::とくぼう |
| BaseStatSpeed | int | NOT NULL | 種族値::すばやさ |
| Weight | int | NOT NULL | 体重 PokeAPIでの値そのまま |
| CreatedAt | timestamp with time zone | NOT NULL, DEFAULT | 作成日時 |
| UpdatedAt | timestamp with time zone | NOT NULL, DEFAULT | 更新日時 |

### Moves（技）

PPと追加効果は後回し。命中率はダメージ計算には不要。

| 列名 | 型 | 制約 | メモ |
| ----- | ----- | ----- | ----- |
| MoveId | int | PK | 技ID PokeAPIでのid |
| MoveName | text | NOT NULL | 技名 日本語名 |
| TypeId | int | FK, NOT NULL | タイプID |
| MoveDamageClassId | int | FK, NOT NULL | 技分類ID |
| Power | int | | 威力 |
| CreatedAt | timestamp with time zone | NOT NULL, DEFAULT | 作成日時 |
| UpdatedAt | timestamp with time zone | NOT NULL, DEFAULT | 更新日時 |

### MoveDamageClasses（技分類）

タイプと同様FK先のための説明用テーブル。

| 列名 | 型 | 制約 | メモ |
| ----- | ----- | ----- | ----- |
| MoveDamageClassId | int | PK | 技分類ID PokeAPIでのid |
| MoveDamageClassName | text | NOT NULL | 技分類名 日本語名 |
| CreatedAt | timestamp with time zone | NOT NULL, DEFAULT | 作成日時 |
| UpdatedAt | timestamp with time zone | NOT NULL, DEFAULT | 更新日時 |

### Abilities（特性）

効果は後回し。

| 列名 | 型 | 制約 | メモ |
| ----- | ----- | ----- | ----- |
| AbilityId | int | PK | 特性ID PokeAPIでのid |
| AbilityName | text | NOT NULL | 特性名 日本語名 |
| CreatedAt | timestamp with time zone | NOT NULL, DEFAULT | 作成日時 |
| UpdatedAt | timestamp with time zone | NOT NULL, DEFAULT | 更新日時 |

### Items（道具）

FlingEffectはダメージ計算には不要。

| 列名 | 型 | 制約 | メモ |
| ----- | ----- | ----- | ----- |
| ItemId | int | PK | 道具ID PokeAPIでのid |
| ItemName | text | NOT NULL | 道具名 日本語名 |
| FlingPower | int | | 投げつけるの威力 投げつけるが失敗する道具はNULL |
| CreatedAt | timestamp with time zone | NOT NULL, DEFAULT | 作成日時 |
| UpdatedAt | timestamp with time zone | NOT NULL, DEFAULT | 更新日時 |

### StatAlignments（能力補正）

能力補正の詳細はコード管理。

| 列名 | 型 | 制約 | メモ |
| ----- | ----- | ----- | ----- |
| StatAlignmentId | int | PK | 能力補正ID PokeAPIでのid |
| StatAlignmentName | text | NOT NULL | 能力補正名 日本語名 |
| CreatedAt | timestamp with time zone | NOT NULL, DEFAULT | 作成日時 |
| UpdatedAt | timestamp with time zone | NOT NULL, DEFAULT | 更新日時 |

## ユーザーデータテーブル

### Individuals（個体）

育成済ポケモン。レベル50固定。能力ポイントバリデーションはコードで。

| 列名 | 型 | 制約 | メモ |
| ----- | ----- | ----- | ----- |
| IndividualId | text | PK | 個体ID prefix\_uuidv7 |
| IndividualName | text | | 個体名 NULLだったら表示時に種族名を表示 |
| SpeciesId | int | FK, NOT NULL | 種族ID |
| StatAlignmentId | int | FK, NOT NULL | 能力補正ID |
| AbilityId | int | FK, NOT NULL | 特性ID |
| StatPointHp | int | NOT NULL | 能力ポイント::HP |
| StatPointAttack | int | NOT NULL | 能力ポイント::こうげき |
| StatPointDefense | int | NOT NULL | 能力ポイント::ぼうぎょ |
| StatPointSpecialAttack | int | NOT NULL | 能力ポイント::とくこう |
| StatPointSpecialDefense | int | NOT NULL | 能力ポイント::とくぼう |
| StatPointSpeed | int | NOT NULL | 能力ポイント::すばやさ |
| Move1Id | int | FK, NOT NULL | 技1ID |
| Move2Id | int | FK | 技2ID |
| Move3Id | int | FK | 技3ID |
| Move4Id | int | FK | 技4ID |
| HeldItemId | int | FK | 持ち物 道具ID |
| TeraTypeId | int | FK, NOT NULL | テラスタイプID |
| Memo | text | | 備考 |
| CategoryId | int | FK, NOT NULL | カテゴリID |
| CreatedAt | timestamp with time zone | NOT NULL, DEFAULT | 作成日時 |
| UpdatedAt | timestamp with time zone | NOT NULL, DEFAULT | 更新日時 |

### IndividualCategories（個体カテゴリ）

手持ち個体かダメージ計算プリセット個体かを区別するためのカテゴリ。タイプと同様FK先のための説明用テーブル。

| 列名 | 型 | 制約 | メモ |
| ----- | ----- | ----- | ----- |
| IndividualCategoryId | int | PK | 個体カテゴリ 連番 |
| IndividualCategoryName | text | NOT NULL | 個体カテゴリ名 |
| CreatedAt | timestamp with time zone | NOT NULL, DEFAULT | 作成日時 |
| UpdatedAt | timestamp with time zone | NOT NULL, DEFAULT | 更新日時 |

### Parties（パーティ）

個体で構成されたパーティ。バトルボックスのイメージ。PartyNameにUNIQUE制約は不要。

| 列名 | 型 | 制約 | メモ |
| ----- | ----- | ----- | ----- |
| PartyId | text | PK | パーティID prefix\_uuidv7 |
| PartyName | text | NOT NULL | パーティ名 |
| Individual1Id | text | FK | 個体1ID |
| Individual2Id | text | FK | 個体2ID |
| Individual3Id | text | FK | 個体3ID |
| Individual4Id | text | FK | 個体4ID |
| Individual5Id | text | FK | 個体5ID |
| Individual6Id | text | FK | 個体6ID |
| Memo | text | | 備考 |
| CreatedAt | timestamp with time zone | NOT NULL, DEFAULT | 作成日時 |
| UpdatedAt | timestamp with time zone | NOT NULL, DEFAULT | 更新日時 |

## スコープ外（後回し）

* 種族技（learnset）
* シーズン
* 戦績・スナップショット関連

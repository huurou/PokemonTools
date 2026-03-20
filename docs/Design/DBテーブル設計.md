# DBテーブル設計

Issue #19 にて確定した決定版。EF Core + PostgreSQL を使用する。

## 設計方針

* **固定長の関係は列で持つ**（種族タイプ×2、特性×3、技×4、パーティスロット×6）
* **タイプ相性はコード管理**（TypeChart）のためDBテーブル不要
* **性格の補正情報はコード管理**。Naturesテーブルは名前解決用
* **努力値バリデーションはアプリ側**で実施（DB制約ではない）
* **ID体系**: マスターデータ系は `int`（PokeAPI ID）、ユーザーデータ系は `text`（prefix\_uuidv7）

## マスターデータテーブル

PokeAPIから取得・変換して保存する。

### Types（ポケモンのタイプ）

| 列名 | 型 | 制約 | メモ |
| ----- | ----- | ----- | ----- |
| TypeId | int | PK | タイプID PokeAPIでのid |
| TypeName | text | NOT NULL | タイプ名 日本語名 |

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

### Moves（技）

PPと追加効果は後回し。命中率はダメージ計算には不要。

| 列名 | 型 | 制約 | メモ |
| ----- | ----- | ----- | ----- |
| MoveId | int | PK | 技ID PokeAPIでのid |
| MoveName | text | NOT NULL | 技名 日本語名 |
| TypeId | int | FK, NOT NULL | タイプID |
| MoveDamageClassId | int | FK, NOT NULL | 技分類ID |
| Power | int | | 威力 |

### MoveDamageClasses（技分類）

タイプと同様FK先のための説明用テーブル。

| 列名 | 型 | 制約 | メモ |
| ----- | ----- | ----- | ----- |
| MoveDamageClassId | int | PK | 技分類ID PokeAPIでのid |
| MoveDamageClassName | text | NOT NULL | 技分類名 日本語名 |

### Abilities（特性）

効果は後回し。

| 列名 | 型 | 制約 | メモ |
| ----- | ----- | ----- | ----- |
| AbilityId | int | PK | 特性ID PokeAPIでのid |
| AbilityName | text | NOT NULL | 特性名 日本語名 |

### Items（道具）

FlingEffectはダメージ計算には不要。

| 列名 | 型 | 制約 | メモ |
| ----- | ----- | ----- | ----- |
| ItemId | int | PK | 道具ID PokeAPIでのid |
| ItemName | text | NOT NULL | 道具名 日本語名 |
| FlingPower | int | | 投げつけるの威力 投げつけるが失敗する道具はNULL |

### Natures（性格）

性格の補正情報はコード管理。

| 列名 | 型 | 制約 | メモ |
| ----- | ----- | ----- | ----- |
| NatureId | int | PK | 性格ID PokeAPIでのid |
| NatureName | text | NOT NULL | 性格名 日本語名 |

## ユーザーデータテーブル

### Individuals（個体）

育成済ポケモン。レベル50固定。努力値バリデーションはコードで。

| 列名 | 型 | 制約 | メモ |
| ----- | ----- | ----- | ----- |
| IndividualId | text | PK | 個体ID prefix\_uuidv7 |
| IndividualName | text | | 個体名 NULLだったら表示時に種族名を表示 |
| SpeciesId | int | FK, NOT NULL | 種族ID |
| NatureId | int | FK, NOT NULL | 性格ID |
| AbilityId | int | FK, NOT NULL | 特性ID |
| IndividualValueHp | int | NOT NULL | 個体値::HP |
| IndividualValueAttack | int | NOT NULL | 個体値::こうげき |
| IndividualValueDefense | int | NOT NULL | 個体値::ぼうぎょ |
| IndividualValueSpecialAttack | int | NOT NULL | 個体値::とくこう |
| IndividualValueSpecialDefense | int | NOT NULL | 個体値::とくぼう |
| IndividualValueSpeed | int | NOT NULL | 個体値::すばやさ |
| EffortValueHp | int | NOT NULL | 努力値::HP |
| EffortValueAttack | int | NOT NULL | 努力値::こうげき |
| EffortValueDefense | int | NOT NULL | 努力値::ぼうぎょ |
| EffortValueSpecialAttack | int | NOT NULL | 努力値::とくこう |
| EffortValueSpecialDefense | int | NOT NULL | 努力値::とくぼう |
| EffortValueSpeed | int | NOT NULL | 努力値::すばやさ |
| Move1Id | int | FK, NOT NULL | 技1ID |
| Move2Id | int | FK | 技2ID |
| Move3Id | int | FK | 技3ID |
| Move4Id | int | FK | 技4ID |
| HeldItemId | int | FK | 持ち物 道具ID |
| TeraTypeId | int | FK, NOT NULL | テラスタイプID |
| Memo | text | | 備考 |
| CategoryId | int | FK, NOT NULL | カテゴリID |

### IndividualCategories（個体カテゴリ）

手持ち個体かダメージ計算プリセット個体かを区別するためのカテゴリ。タイプと同様FK先のための説明用テーブル。

| 列名 | 型 | 制約 | メモ |
| ----- | ----- | ----- | ----- |
| IndividualCategoryId | int | PK | 個体カテゴリ 連番 |
| IndividualCategoryName | text | NOT NULL | 個体カテゴリ名 |

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

## スコープ外（後回し）

* 種族技（learnset）
* シーズン
* 戦績・スナップショット関連

# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## プロジェクト概要

Pokemon Champions専用の対戦補助Webアプリケーション（[ADR-0005](./docs/ADR/adr-0005-Pokemon-Champions専用ツールとする.md)）。ダメージ計算・実数値計算・タイプ相性などの機能を提供する。過去世代互換や将来作品を見越した汎用抽象化は初期スコープに含めない。

## ビルド・実行コマンド

※ 以下のコマンドはすべてリポジトリルートから実行する。

```bash
# アプリ全体の起動（Aspireオーケストレーション）
aspire run

# ビルド
dotnet build PokemonTools.slnx

# 全テスト実行
dotnet test --solution PokemonTools.slnx

# 単一テストクラスの実行（MTP v2 では --project が必須）
dotnet test --project tests/PokemonTools.Web.Domain.Tests --filter "FullyQualifiedName~DamageCalculator_CalculateTests"

# 単一テストメソッドの実行
dotnet test --project tests/PokemonTools.Web.Domain.Tests --filter "FullyQualifiedName~メソッド名"

# ローカルツールのリストア（dotnet-ef等）
dotnet tool restore

# マイグレーション追加
dotnet ef migrations add <MigrationName> --project src/PokemonTools.Web.Infrastructure --startup-project src/PokemonTools.Web

# マイグレーション適用（通常はアプリ起動時に自動実行）
dotnet ef database update --project src/PokemonTools.Web.Infrastructure --startup-project src/PokemonTools.Web

# カバレッジレポート生成（要 dotnet-reportgenerator-globaltool）
pwsh scripts/TestCoverage.ps1

# CSS ビルド（Tailwind）
npm run css:build

# CSS ウォッチ（開発時）
npm run css:watch
```

## CSS

- **Tailwind CSS v4** でスタイリング。CSS isolationファイル (.razor.css) は使用しない
- Tailwindソース: `src/PokemonTools.Web/Styles/app.css`
- ビルド出力: `src/PokemonTools.Web/wwwroot/app.generated.css` (git管理外)
- MSBuild統合: `build/Tailwind.targets` でビルド/パブリッシュ時に自動生成
- Bootstrapクラスと同名のコンポーネントクラス (`btn`, `form-control`, `alert` 等) は `@layer components` で定義済み

## アーキテクチャ

- **.NET 10** / **Aspire** によるオーケストレーション
- **クリーンアーキテクチャ**: Infrastructure / Application → Domain の依存方向（Domain層が最内層、外側から内側への依存のみ許可）
- **単一Webサービス構成**: Blazor SSR（Web）がApplication/Infrastructureを経由してドメインロジック・DB・外部APIの機能を統合。AppHostがPostgreSQLとWebをオーケストレーション

### プロジェクト依存関係

```
AppHost
├── PostgreSQL ("pokemon-tools-db")
├── Web ──→ Web.Application → Web.Domain
│    │      Web.Infrastructure → Web.Domain
│    ├──→ PostgreSQL（WaitFor）
│    └──→ ServiceDefaults
└── ServiceDefaults（OpenTelemetry、ヘルスチェック、サービスディスカバリ）
```

- Web.Application は現在空で、ユースケース定義の準備段階
- WebはDomain層を直接参照せず、Application/Infrastructureを経由してのみ依存する（クリーンアーキテクチャの依存ルール）

### 横断的関心事の配置

- **ServiceDefaults**: OpenTelemetry・ヘルスチェック・サービスディスカバリ・レジリエンスなどの横断的関心事を集約する。Webは`AddServiceDefaults()`を呼ぶだけでこれらを取得する
- **AppHost**: サービスの依存関係と起動順序を管理する。PostgreSQL→Webの順序保証（`WaitFor`+ヘルスチェック）はここで定義する
- **Infrastructure層**: 外部APIアクセス（PokeAPI等）はInfrastructure層に閉じ込める。DI登録は層ごとの拡張メソッド（例: `AddPokeApiClient()`）で隠蔽し、ホスト側は実装詳細を知らない

### ドメイン層の境界付きコンテキスト（Web.Domain）

| コンテキスト | 内容 |
|---|---|
| `Abilities/` | 特性（Ability + AbilityId）。マスターデータ |
| `Damages/` | ダメージ計算エンジン。Q12形式（12bit固定小数点）で精密計算 |
| `Individuals/` | 育成済みポケモン個体（Individual）。IndividualCategory（手持ち/ダメージ計算プリセット）で分類。ユーザーデータ |
| `Items/` | 道具（Item + ItemId）。投げつける威力も保持。マスターデータ |
| `Moves/` | 技の実体モデル（Move: タイプ・分類・威力）と技分類（MoveDamageClass: へんか・ぶつり・とくしゅ）。マスターデータ |
| `Parties/` | パーティ（最大6体の個体スロット）。ユーザーデータ |
| `Species/` | ポケモン種族（PokemonSpecies: タイプ・特性・種族値・体重）+ Weight値オブジェクト。マスターデータ |
| `Statistics/` | 能力値関連の値オブジェクト群（StatPoints, BaseStats, StatAlignment等）と実数値計算エンジン（StatsCalculator） |
| `Types/` | 18タイプ相性表＋ステラ・???（相性未定義、等倍扱い）。FrozenDictionaryで管理。デュアルタイプ対応 |
| `Utility/` | Q12演算の拡張メソッド（四捨五入・五捨五超入）とdouble→uint変換 |

### Q12固定小数点演算

ダメージ計算はポケモン本家の仕様に準拠し、補正値の合成と丸めをQ12形式（4096ベース）の整数演算で行う。補正値を`uint`のまま連鎖適用し、各段階でQ12丸めを行うことで本家と同一の計算結果を再現する。

- `Q12Round`（四捨五入）: 端数 `< 0x0800` なら切り捨て、それ以上は切り上げ
- `Q12RoundHalfDown`（五捨五超入）: 端数 `<= 0x0800` なら切り捨て、超えたら切り上げ

`DamageCalculator`は8段階のパイプラインで計算する: 威力補正 → 威力 → 攻撃補正 → 攻撃 → 防御補正 → 防御 → ダメージ補正 → 最終ダメージ（乱数16通り）

### Infrastructure層の設計方針

- 外部API（PokeAPI v2）へのアクセスはInfrastructure層に閉じ込め、Domain層は外部依存を持たない
- PokeAPIはフェアユースポリシー遵守のため、リクエスト間隔を最低200msに制限する（`PokeApiRequestLimiter`）
- テスト可能性のため`TimeProvider`をDI注入し、テストでは`FakeTimeProvider`に差し替える
- **永続化**: EF Core + Npgsql（PostgreSQL）。Aspire統合（`AddNpgsqlDbContext`）でDbContextを登録
- **エンティティ設計**: 各エンティティは`IEntityTypeConfiguration<T>`でFluent API設定。`IHasUpdatedAt`実装エンティティは`TimestampSaveChangesInterceptor`でUpdatedAtを自動管理。CreatedAtはDBデフォルト値（`CURRENT_TIMESTAMP`）で設定
- **ID体系**: マスターデータはPokeAPI準拠の`int`、ユーザーデータは`prefix_uuidv7`形式の`string`

## コーディング規約

- プライベートフィールドはキャメルケース + `_` サフィックス（例: `rands_`）
- 定数（`const`）は `UPPER_SNAKE_CASE` を使用（例: `PAGE_SIZE`）
- LINQの引数には `x`, `y`, `z` を使用
- 値オブジェクトは `record` で定義し、必要に応じてコンストラクタでガード節（`ArgumentOutOfRangeException.ThrowIf*`等）によるバリデーション
- `Pokemon` プリフィックスは名前空間や `System.Type` との衝突回避が必要な場合のみ付与（例: `PokemonType`, `PokemonSpecies`）。衝突がないクラス（`Move`, `Ability`等）には付けない
- 状態や外部依存を持たないドメインサービス（`DamageCalculator`, `StatsCalculator`, `TypeChart`）は`static`クラスとして実装
- 別集約のドメインモデルはID値オブジェクト（例: `SpeciesId`, `AbilityId`）で参照し、実体を直接参照しない
- 有限個のインスタンスを持つドメインモデル（`StatAlignment`, `PokemonType`, `MoveDamageClass`, `IndividualCategory`等）はシングルトンプロパティ + `static ImmutableArray<T> All` パターンで実装
- 固定スロット数が決まっている概念（技4枠、パーティ6枠等）は`List`ではなく番号付きプロパティ（`Move1Id`〜`Move4Id`等）で表現

## テスト規約

- **フレームワーク**: xUnit v3（Fluent Assertionは使用しない）
- **テストランナー**: Microsoft Testing Platform (MTP v2) — `global.json`で設定済み
- **テストクラス名**: `{対象クラス}_{対象メソッド}Tests`（例: `DamageCalculator_CalculateTests`）
- **テストメソッド名**: 日本語で「条件_期待される挙動」（例: `急所かつ攻撃ランクがマイナス_マイナスランクが無視されランク0相当のダメージが返る`）
- **構造**: AAA（Arrange-Act-Assert）パターン
- **例外テスト**: `Assert.Throws`ではなく`Record.Exception()` / `Record.ExceptionAsync()` + `Assert.IsType<>()`でAct/Assertを分離する
- **ドキュメントコメント**: テストコードには付与しない
- **ヘルパーパターン**: テストクラスにはデフォルト値付きの`private static`ヘルパーメソッド（例: `CalculateWithDefaults()`）を定義し、テストメソッドでは変更したいパラメータのみnamed argumentで指定する
- **カバレッジ**: ブランチカバレッジ100%を目標とする。カバレッジレポートのアセンブリフィルタはエントリポイント（Web）を除外し、Domain/Infrastructure/Applicationのみ計測する
- **テストプロジェクト設定**: MTP v2ランナーのため`OutputType`は`Exe`が必須。パッケージは`xunit.v3.mtp-v2` + `Microsoft.Testing.Extensions.CodeCoverage`
- **テストプロジェクト構成**: 以下の3系統
  - `Web.Domain.Tests` — ドメインロジックの単体テスト
  - `Web.Infrastructure.Tests` — 外部依存（PokeAPIクライアント等）のテスト。実ネットワークに出ず、偽のHTTPハンドラと`FakeTimeProvider`でレート制限・ページング・キャンセルを決定論的に検証する
  - `PokemonTools.Tests` — Aspireホスト統合テスト（PostgreSQLコンテナを使用するためDocker必須）

## リポジトリ不変条件

- **エンコーディング**: `.cs` / `.xaml` ファイルはUTF-8 BOM付き・CRLF改行で管理する。Claude Code の Stop フックで `scripts/ConvertToUtf8Bom.ps1` が自動実行され、これを強制する

## CI/CD

- `.github/workflows/dotnet-test.yml` — push/PR時にビルド・テスト自動実行（ubuntu-latest, .NET 10）
- `.github/workflows/claude.yml` — `@claude`メンション時にClaude Code Actionが起動
- `.github/workflows/claude-code-review.yml` — PR作成・更新時にClaude Code Reviewが自動実行

## 参考ドキュメント

- `docs/ADR/` — アーキテクチャ決定記録
  - ADR-0001: タイプのクラス名について（`PokemonType`の命名理由）
  - ADR-0002: 種族の名前空間・クラス名について（`PokemonSpecies`の命名理由）
  - ADR-0003: Pokemonプリフィックスの使用について（プリフィックス付与基準）
  - ADR-0004: 努力値・個体値vs能力ポイント（StatPoints/StatAlignment採用の方針）
  - ADR-0005: Pokemon Champions専用ツールとする（スコープの正式決定、過去世代互換は対象外）
- `docs/Requirements/` — 要件定義書
- `docs/Design/` — DBテーブル設計書（エンティティ定義・リレーション・制約の詳細）

# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## プロジェクト概要

ポケモン対戦（ランクマッチ / Gen9 SV）を補助するWebアプリケーション。ダメージ計算・実数値計算・タイプ相性などの機能を提供する。

## ビルド・実行コマンド

```bash
# アプリ全体の起動（Aspireオーケストレーション）
aspire run

# ビルド
dotnet build PokemonTools.slnx

# 全テスト実行
dotnet test PokemonTools.slnx

# 単一テストクラスの実行
dotnet test tests/PokemonTools.ApiService.Domain.Tests --filter "FullyQualifiedName~DamageCalculator_CalculateTests"

# 単一テストメソッドの実行
dotnet test tests/PokemonTools.ApiService.Domain.Tests --filter "FullyQualifiedName~メソッド名"

# カバレッジレポート生成（要 dotnet-reportgenerator-globaltool）
pwsh scripts/TestCoverage.ps1
```

## アーキテクチャ

- **.NET 10** / **Aspire** によるマルチサービスオーケストレーション
- **クリーンアーキテクチャ**: Domain → Application → Infrastructure の依存方向
- **2サービス構成**: ApiService（Web API）と Web（Blazor SSR）。AppHostが両者をオーケストレーション
- Web → ApiService 間はAspireのサービスディスカバリで接続（`https+http://apiservice`）
- AppHostは`WaitFor`でApiServiceのヘルスチェック通過を待ってからWebを開始する

### プロジェクト依存関係

```
AppHost
├── ApiService ──→ Application → Domain
│    │             Infrastructure → Domain
│    └──→ ServiceDefaults
├── Web ──→ Web.Application → Web.Domain
│    │      Web.Infrastructure → Web.Domain
│    └──→ ServiceDefaults
└── ServiceDefaults（両サービス共通：OpenTelemetry、ヘルスチェック、サービスディスカバリ）
```

### ドメイン層の境界付きコンテキスト（ApiService.Domain）

| コンテキスト | 内容 |
|---|---|
| `Damages/` | ダメージ計算エンジン。Q12形式（12bit固定小数点）で精密計算 |
| `Statistics/` | 実数値計算（HP/攻撃/防御/特攻/特防/素早さ）。種族値・個体値・努力値・性格補正対応 |
| `Types/` | 18タイプ相性表。FrozenDictionaryで管理。デュアルタイプ対応 |
| `Utility/` | Q12演算の拡張メソッド（四捨五入・五捨五超入） |

### Q12固定小数点演算

ダメージ計算はポケモン本家の仕様に準拠し、補正値の合成と丸めをQ12形式（4096ベース）の整数演算で行う。補正値を`uint`のまま連鎖適用し、各段階でQ12丸めを行うことで本家と同一の計算結果を再現する。

- `Q12Round`（四捨五入）: 端数 `< 0x0800` なら切り捨て、それ以上は切り上げ
- `Q12RoundHalfDown`（五捨五超入）: 端数 `<= 0x0800` なら切り捨て、超えたら切り上げ

`DamageCalculator`は8段階のパイプラインで計算する: 威力補正 → 威力 → 攻撃補正 → 攻撃 → 防御補正 → 防御 → ダメージ補正 → 最終ダメージ（乱数16通り）

## コーディング規約

- プライベートフィールドはキャメルケース + `_` サフィックス（例: `rands_`）
- LINQの引数には `x`, `y`, `z` を使用
- 値オブジェクトは `record` で定義し、必要に応じてコンストラクタでガード節（`ArgumentOutOfRangeException.ThrowIf*`等）によるバリデーション
- `Pokemon` プリフィックスは名前空間や `System.Type` との衝突回避が必要な場合のみ付与（例: `PokemonType`, `PokemonSpecies`）。衝突がないクラス（`Move`, `Ability`等）には付けない
- 状態や外部依存を持たないドメインサービス（`DamageCalculator`, `StatsCalculator`, `TypeChart`）は`static`クラスとして実装

## テスト規約

- **フレームワーク**: xUnit v3（Fluent Assertionは使用しない）
- **テストランナー**: Microsoft Testing Platform (MTP v2) — `global.json`で設定済み
- **テストクラス名**: `{対象クラス}_{対象メソッド}Tests`（例: `DamageCalculator_CalculateTests`）
- **テストメソッド名**: 日本語で「条件_期待される挙動」（例: `急所かつ攻撃ランクがマイナス_マイナスランクが無視されランク0相当のダメージが返る`）
- **構造**: AAA（Arrange-Act-Assert）パターン
- **例外テスト**: `Assert.Throws`ではなく`Record.Exception()` / `Record.ExceptionAsync()` + `Assert.IsType<>()`でAct/Assertを分離する
- **ドキュメントコメント**: テストコードには付与しない
- **テストプロジェクト構成**: `ApiService.Domain.Tests`（ドメインロジックの単体テスト）と `PokemonTools.Tests`（Aspireホスト統合テスト）の2系統

## CI/CD

- `.github/workflows/dotnet-test.yml` — push/PR時にビルド・テスト自動実行（ubuntu-latest, .NET 10）
- `.github/workflows/claude.yml` — `@claude`メンション時にClaude Code Actionが起動
- `.github/workflows/claude-code-review.yml` — PR作成・更新時にClaude Code Reviewが自動実行

## 参考ドキュメント

- `docs/ADR/` — アーキテクチャ決定記録（クラス命名規則等）
- `docs/Requirements/` — 要件定義書

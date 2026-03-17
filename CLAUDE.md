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

### ドメイン層の境界付きコンテキスト（ApiService.Domain）

| コンテキスト | 内容 |
|---|---|
| `Damages/` | ダメージ計算エンジン。Q12形式（12bit固定小数点）で精密計算 |
| `Statistics/` | 実数値計算（HP/攻撃/防御/特攻/特防/素早さ）。種族値・個体値・努力値・性格補正対応 |
| `Types/` | 18タイプ相性表。FrozenDictionaryで管理。デュアルタイプ対応 |
| `Utility/` | Q12演算の拡張メソッド（四捨五入・五捨五超入） |

## コーディング規約

- プライベートフィールドはキャメルケース + `_` サフィックス（例: `rands_`）
- LINQの引数には `x`, `y`, `z` を使用
- 値オブジェクトは `record` で定義し、コンストラクタでバリデーション
- `Pokemon` プリフィックスは名前空間や `System.Type` との衝突回避が必要な場合のみ付与（例: `PokemonType`, `PokemonSpecies`）。衝突がないクラス（`Move`, `Ability`等）には付けない

## テスト規約

- **フレームワーク**: xUnit v3（Fluent Assertionは使用しない）
- **テストクラス名**: `{対象クラス}_{対象メソッド}Tests`（例: `DamageCalculator_CalculateTests`）
- **テストメソッド名**: 日本語で「条件_期待される挙動」（例: `急所かつ攻撃ランクがマイナス_マイナスランクが無視されランク0相当のダメージが返る`）
- **構造**: AAA（Arrange-Act-Assert）パターン
- **ドキュメントコメント**: テストコードには付与しない

## 参考ドキュメント

- `docs/ADR/` — アーキテクチャ決定記録（クラス命名規則等）
- `docs/Requirements/` — 要件定義書

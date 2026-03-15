# 作業メモ

## プロジェクト作成

Aspire CLI インストール

[Aspire CLI をインストールする | Aspire](https://aspire.dev/ja/get-started/install-cli/)

```bash
irm https://aspire.dev/install.ps1 | iex
```

以下のコードを実行

- テンプレートからプロジェクト作成
  - テストプロジェクト追加
    - xUnit.Net v3
- .gitignore追加
- パッケージアップデート
- slnx作成(slnからのマイグレート)
- sln削除
- README.md作成
- srcフォルダ作成
- git初期化

```bash
aspire new aspire-starter -n PokemonTools -o PokemonTools &&
cd PokemonTools &&
dotnet new gitignore &&
dotnet package update &&
dotnet sln migrate &&
rm PokemonTools.sln &&
echo "# PokemonTools" > README.md &&
mkdir src &&
git init
```

以下を手作業で実施

- プロジェクトフォルダをsrcフォルダに移動
- slnxの中身修正
  - 各プロジェクトファイルのパスの前に `src/` を追加
- .gitignore に `.aspire/` を追加
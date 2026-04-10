# app.css Bootstrap互換CSS → Tailwindユーティリティ全面移行

## 目的

`src/PokemonTools.Web/Styles/app.css` の `@layer components` に残っているBootstrap互換CSSクラス（約380行）をすべて削除し、各razorファイルでTailwindユーティリティクラスに置き換える。

## 現状

app.cssの `@layer components` に以下のBootstrap互換クラスが定義されている：

| カテゴリ | クラス | 主な使用ファイル |
|---------|--------|-----------------|
| Buttons | btn, btn-primary/secondary/danger, btn-outline-*, btn-sm, btn-close | 5ファイル（IndividualList, IndividualForm, MasterDataImport, IndividualEdit, Counter） |
| Forms | form-label, form-control, form-select | IndividualForm（集中） |
| Alerts | alert, alert-danger/warning/success/info | IndividualList, MasterDataImport, IndividualEdit, IndividualForm |
| Card | card, card-header, card-body | IndividualForm |
| Table | table, table-sm, table-striped | IndividualList, IndividualForm |
| Modal | modal, modal-dialog/content/header/title/body/footer | IndividualList |
| Progress | progress, progress-bar, progress-bar-striped/animated | MasterDataImport |
| Sidebar | sidebar-nav-link, sidebar-nav-icon | NavMenu |
| Blazor UI | #blazor-error-ui | MainLayout |

また `@utility` セクションに `text-danger`, `text-muted`, `d-block` がある。

## 移行方針

### 画面単位で移行し、未使用になったクラスから削除する

app.cssのクラス定義を先に消さないこと。razorファイルを1つずつTailwind化し、全使用箇所が消えたクラスからapp.cssの定義を削除する。

### カテゴリ別の方針

#### 1. sidebar-nav-link / sidebar-nav-icon → インラインTailwind

NavMenu.razorでのみ使用。`[&.active]:` でBlazor NavLinkのactive状態に対応する。

```razor
<!-- 例 -->
<NavLink class="flex items-center h-12 px-3 rounded text-[#d7d7d7] no-underline
                hover:bg-white/10 hover:text-white
                [&.active]:bg-white/[0.37] [&.active]:text-white"
         href="" Match="NavLinkMatch.All">
    <svg class="w-5 h-5 mr-3 -mt-px fill-white shrink-0" viewBox="0 0 16 16">
```

#### 2. modal → インラインTailwind

IndividualList.razorでのみ使用。直接Tailwindに置き換え。

#### 3. progress → インラインTailwind

MasterDataImport.razorでのみ使用。ストライプアニメーションは `@keyframes` をapp.cssに残すか、Tailwindの `animate-` カスタムで対応。

#### 4. alert → インラインTailwind

各画面で直接Tailwindクラスに置き換え。使用箇所が少ないのでコンポーネント化不要。

#### 5. card / table → インラインTailwind

使用箇所が限られるため直接置き換え。

#### 6. form-label / form-control / form-select → インラインTailwind

IndividualForm.razorに集中。form-selectのドロップダウン矢印SVGは `bg-[url(...)]` で対応。

#### 7. btn系 → Razorコンポーネント化

btn系は5ファイルで使われ、バリアント（primary/secondary/danger/outline-*）・サイズ（sm）・状態（disabled）・要素種別（button/a）の組み合わせがある。毎回ユーティリティをベタ書きすると各所で微妙にズレるリスクがあるため、以下のいずれかで共通化する：

- **案A**: `Components/Shared/Button.razor` コンポーネントを作成し、Variant/Size等をパラメータ化
- **案B**: 静的クラスにTailwindクラス文字列の定数を定義し、razorから参照

案Aを推奨。

#### 8. #blazor-error-ui → インラインTailwind

MainLayout.razorの `<div id="blazor-error-ui">` にTailwindクラスを直接指定。

#### 9. @layer base のバリデーションスタイル → フォームコンポーネントに吸収

Blazorが自動付与する `.valid.modified`, `.invalid`, `.validation-message` はCSS側で定義せず、フォーム入力コンポーネント側で `[&.class]:` 記法を使って対応する。

```razor
<!-- 入力コンポーネントの例 -->
<InputText class="block w-full px-3 py-1.5 border border-gray-300 rounded-md
                  focus:border-blue-400 focus:ring-2 focus:ring-blue-300 focus:outline-none
                  [&.valid.modified:not([type=checkbox])]:outline-1 [&.valid.modified:not([type=checkbox])]:outline-green-600
                  [&.invalid]:outline-1 [&.invalid]:outline-red-600"
           @bind-Value="Value" />

<!-- ValidationMessageにはclass属性を直接渡す -->
<ValidationMessage For="() => Model.Name" class="text-red-600 text-sm" />
```

### 残すもの（移行しない）

- **`@layer base`の`.blazor-error-boundary`**: `<ErrorBoundary>`コンポーネントが内部で使用するクラス。Tailwind化不可。最小限のCSS（4行）なので残す
- **`@layer base`のhtml,body / a / h1:focus**: グローバルリセット。残す
- **`@theme`**: Tailwindのテーマ変数定義。そのまま残す

### 削除するもの

- **`@layer base`の`.valid.modified` / `.invalid` / `.validation-message`**: フォームコンポーネント側で吸収するため削除
- **`@layer components`**: 全クラスをrazor側に移行後、セクションごと削除
- **`@utility d-block`**: modalのTailwind化後に不要になるため削除
- **`@utility text-danger / text-muted`**: 使用箇所でTailwindの色指定に置き換え後、削除

## 実行順序

1. NavMenu.razor（sidebar-nav-link / sidebar-nav-icon）
2. IndividualList.razor（modal, table, alert, btn）
3. MasterDataImport.razor（progress, alert, btn）
4. IndividualForm.razor（form-control, form-select, form-label, card, table, alert, btn）
5. IndividualEdit.razor（alert, btn）
6. Counter.razor（btn）
7. MainLayout.razor（#blazor-error-ui）
8. btn系のRazorコンポーネント化（上記2〜6でbtnを仮置きしていた場合、コンポーネントに置き換え）
9. フォーム入力コンポーネント化（バリデーションスタイルを吸収）
10. app.cssから `@layer components` を削除、`@layer base` からバリデーション関連を削除、不要な `@utility` を削除

## 検証

- 各ステップ後に `dotnet build PokemonTools.slnx` でビルド確認
- 各ステップ後にブラウザで該当画面の見た目を目視確認（色・サイズ・hover・active・disabled状態）
- 全ステップ完了後に `dotnet test --solution PokemonTools.slnx` でテスト通過確認
- app.cssの `@layer components` が空（もしくは削除済み）であることを確認

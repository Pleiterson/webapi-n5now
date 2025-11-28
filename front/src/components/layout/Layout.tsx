import Header from "./Header";
import "./Layout.scss";

export default function Layout({ children } : { children: React.ReactNode }) {
  return (
    <div className="layout">
      <Header />

      <main className="layout__content">
        { children }
      </main>
    </div>
  );
}

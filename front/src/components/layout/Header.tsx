import { Link } from "react-router-dom";
import logo from "../../assets/n5now_logo.jpeg";
import "./Header.scss";

export default function Header() {
  return (
    <header className="header">
      <div className="header__logo">
        <img src={ logo } alt="N5 Now Logo" className="header__logo-img" />
      </div>

      <nav className="header__nav">
        <Link to="/permissions">Permissions</Link>
        <Link to="/types">Permission Types</Link>
      </nav>
    </header>
  );
}

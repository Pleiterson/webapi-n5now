import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import Layout from "./components/layout/Layout";
import { PermissionsPage } from "./pages/Permissions/PermissionsPage";
import PermissionTypesPage from "./pages/PermissionTypes/PermissionTypesPage";

function App() {
  return (
    <BrowserRouter>
      <Layout>
        <Routes>
          <Route path="/" element={ <Navigate to="/permissions" /> } />
          <Route path="/permissions" element={ <PermissionsPage /> } />
          <Route path="/types" element={ <PermissionTypesPage /> } />
        </Routes>
      </Layout>
    </BrowserRouter>
  );
}

export default App;

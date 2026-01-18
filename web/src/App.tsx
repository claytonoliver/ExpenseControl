import { BrowserRouter as Router, Routes, Route, NavLink } from 'react-router-dom';
import { PersonsPage, CategoriesPage, TransactionsPage, ReportsPage } from './pages';

/**
 * Componente principal da aplicação.
 * Define a navegação e rotas do sistema de controle de gastos.
 */
function App() {
  return (
    <Router>
      <div className="app-container">
        {/* Navegação principal */}
        <nav className="navbar">
          <h1>Controle de Gastos Residenciais</h1>
          <div className="nav-links">
            <NavLink to="/" className={({ isActive }) => (isActive ? 'active' : '')}>
              Pessoas
            </NavLink>
            <NavLink to="/categorias" className={({ isActive }) => (isActive ? 'active' : '')}>
              Categorias
            </NavLink>
            <NavLink to="/transacoes" className={({ isActive }) => (isActive ? 'active' : '')}>
              Transações
            </NavLink>
            <NavLink to="/relatorios" className={({ isActive }) => (isActive ? 'active' : '')}>
              Relatórios
            </NavLink>
          </div>
        </nav>

        {/* Conteúdo das rotas */}
        <main>
          <Routes>
            <Route path="/" element={<PersonsPage />} />
            <Route path="/categorias" element={<CategoriesPage />} />
            <Route path="/transacoes" element={<TransactionsPage />} />
            <Route path="/relatorios" element={<ReportsPage />} />
          </Routes>
        </main>
      </div>
    </Router>
  );
}

export default App;

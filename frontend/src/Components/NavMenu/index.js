import { Navbar, Nav } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap'

export default function NavMenu() {
    return(
        <Navbar variant="dark" bg="dark" expand="lg">
            <LinkContainer to="/">
                <Navbar.Brand href="/">ForLogic</Navbar.Brand>
            </LinkContainer>
            <Navbar.Toggle aria-controls="basic-navbar-nav" />
            <Navbar.Collapse id="basic-navbar-nav">
                <Nav className="mr-auto">
                    <LinkContainer to="/" exact>
                        <Nav.Link href="/">Avaliações</Nav.Link>
                    </LinkContainer>
                    <LinkContainer to="/clientes">
                        <Nav.Link href="/clientes">Clientes</Nav.Link>
                    </LinkContainer>
                </Nav>
            </Navbar.Collapse>
        </Navbar>
    );
}
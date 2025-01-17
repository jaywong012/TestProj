import React from "react";
import { Navbar, Nav, Container, Button } from "react-bootstrap";
import { BoxArrowLeft } from "react-bootstrap-icons";
import { Link, useNavigate } from "react-router-dom";

const MyNavbar = () => {
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem("token");
    navigate("/login");
  };
  return (
    <Navbar bg="light" expand="lg">
      <Container>
        <Navbar.Brand as={Link} to="/">
          Test Page
        </Navbar.Brand>
        <Navbar.Toggle aria-controls="navbar-nav" />
        <Navbar.Collapse id="navbar-nav">
          <Nav className="me-auto">
            <Nav.Link as={Link} to="/">
              Product
            </Nav.Link>
            <Nav.Link as={Link} to="/category">
              Category
            </Nav.Link>
          </Nav>
          {localStorage.getItem("token") && (
            <Nav>
              <Nav.Item>
                <Button variant="outline-danger" onClick={handleLogout}>
                  <BoxArrowLeft/>
                </Button>
              </Nav.Item>
            </Nav>
          )}
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
};

export default MyNavbar;

import SpinnerComponent from "./Spinner";
import { render, screen } from "@testing-library/react";

describe("SpinnerComponent", () => {
    test("Test if component with status role and text loading is exist", () => {
        render(<SpinnerComponent />);

        const spinner = screen.getByRole("status");
        expect(spinner).toBeInTheDocument();

        const loadingText = screen.getByText("Loading...");
        expect(loadingText).toBeInTheDocument();
    });
});
import { fireEvent, render, screen } from "@testing-library/react";
import React from "react";
import '@testing-library/jest-dom';
import CustomButton from "./CustomButton";
import { jest } from '@jest/globals';

describe("CustomButton", () => {
    test('renders with default "Add" text and primary variant', () => {
        render(<CustomButton action='ADD'/>);
        const button = screen.getByRole('button', {name: /add/i});
        expect (button).toBeInTheDocument();
        expect(button).toHaveClass('btn-primary');
    });

    test('renders with "Edit" text and primary variant', () => {
        render(<CustomButton action='EDIT'/>);
        const button = screen.getByRole('button', {name: /edit/i});
        expect (button).toBeInTheDocument();
        expect(button).toHaveClass('btn-primary');
    });

    test('renders with "Cancel" text and danger variant', () => {
        render(<CustomButton action='CANCEL'/>);
        const button = screen.getByRole('button', {name: /cancel/i});
        expect (button).toBeInTheDocument();
        expect(button).toHaveClass('btn-danger');
    });

    test('renders with "Submit" text', () => {
        const handleClick = jest.fn();
        render(<CustomButton action='ADD' onClick={handleClick}/>);
        const button = screen.getByRole('button', {name: /add/i});
        fireEvent.click(button);
        expect(handleClick).toHaveBeenCalledTimes(1);
    });
});
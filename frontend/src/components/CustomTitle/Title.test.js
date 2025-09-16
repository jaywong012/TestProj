import React from 'react';
import {render, screen} from '@testing-library/react';
import '@testing-library/jest-dom';
import Title from './CustomTitle';

describe('Title', () => {
    test('renders with or without a children', () => {
        render(<Title />);
    });

    test('renders children', () => {
        render(<Title>Test</Title>);
        expect(screen.getByText('Test')).toBeInTheDocument();
    });

    test('renders children with textCase', () => {
        render(<Title textCase="uppercase">Test</Title>);
        expect(screen.getByText('Test')).toHaveStyle('text-transform: uppercase');
    });

    test('renders children with color', () => {
        render(<Title color="red">Test</Title>);
        expect(screen.getByText('Test')).toHaveStyle('color: red');
    });
});
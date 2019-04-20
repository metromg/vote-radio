import React from 'react';
import { shallow, mount } from 'enzyme';
import { CoverBackgroundImage } from './CoverBackgroundImage';

describe('CoverBackgroundImage', () => {
    it('renders without crashing', () => {
        shallow(<CoverBackgroundImage url={null}><span>Test</span></CoverBackgroundImage>);
    });

    describe('with no url', () => {
        it('displays children', () => {
            // Arrange
            const element = <CoverBackgroundImage url={null}><span>Test</span></CoverBackgroundImage>;

            // Act
            const result = mount(element);

            // Assert
            expect(result.html()).toContain("<span>Test</span>");
        });

        it('has no background', () => {
            // Arrange
            const element = <CoverBackgroundImage url={null}><span>Test</span></CoverBackgroundImage>;

            // Act
            const result = mount(element);

            // Assertion
            const backgroundCover = result.find('.background-cover');

            expect(backgroundCover.hasClass('has-background')).toBe(false);
            expect(backgroundCover.get(0).props.style).toHaveProperty('backgroundImage', undefined);
        });
    });

    describe('with url', () => {
        it('displays children', () => {
            // Arrange
            const element = <CoverBackgroundImage url={"http://test.example.org"}><span>Test</span></CoverBackgroundImage>;

            // Act
            const result = mount(element);

            // Assert
            expect(result.html()).toContain("<span>Test</span>");
        });

        it('has background', () => {
            // Arrange
            const element = <CoverBackgroundImage url={"http://test.example.org"}><span>Test</span></CoverBackgroundImage>;

            // Act
            const result = mount(element);

            // Assertion
            const backgroundCover = result.find('.background-cover');

            expect(backgroundCover.hasClass('has-background')).toBe(true);
            expect(backgroundCover.get(0).props.style).toHaveProperty('backgroundImage', 'url(http://test.example.org)');
        });
    });
});
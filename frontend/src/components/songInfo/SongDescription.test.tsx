import React from 'react';
import { shallow, mount } from 'enzyme';
import { SongDescription } from './SongDescription';

describe('SongDescription', () => {
    it('renders without crashing', () => {
        shallow(<SongDescription title={'Test'} />);
    });

    describe('with only title', () => {
        it('renders only title', () => {
            // Arrange
            const element = <SongDescription title="Test Title" />;

            // Act
            const result = mount(element);

            // Assert
            const title = result.find('.title');

            expect(title.text()).toBe('Test Title');
            expect(result.html()).not.toContain('subtitle');
        });
    });

    describe('with title and album', () => {
        it('renders title and album', () => {
            // Arrange
            const element = <SongDescription title="Test Title" album="Test Album" />;

            // Act
            const result = mount(element);

            // Assert
            const title = result.find('.title');
            const subtitle = result.find('.subtitle');

            expect(title.text()).toBe('Test Title');
            expect(subtitle.text()).toBe('Test Album');
        });
    });

    describe('with title and artist', () => {
        it('renders title and artist', () => {
            // Arrange
            const element = <SongDescription title="Test Title" artist="Test Artist" />;

            // Act
            const result = mount(element);

            // Assert
            const title = result.find('.title');
            const subtitle = result.find('.subtitle');

            expect(title.text()).toBe('Test Title');
            expect(subtitle.text()).toBe('Test Artist');
        });
    });

    describe('with title, album and artist', () => {
        it('renders title, album and artist', () => {
            // Arrange
            const element = <SongDescription title="Test Title" album="Test Album" artist="Test Artist" />;

            // Act
            const result = mount(element);

            // Assert
            const title = result.find('.title');
            const subtitle = result.find('.subtitle');

            expect(title.text()).toBe('Test Title');
            expect(subtitle.text()).toBe('Test Artist - Test Album');
        });
    });
});
<!-- SEED: re-run /impeccable document once there's code to capture the actual tokens and components. -->

---
name: TouchGrass
description: A personal bucket list and life goals tracker — calm, minimal, and yours.
---

# Design System: TouchGrass

## 1. Overview

**Creative North Star: "The Quiet Notebook"**

TouchGrass lives in the space between intention and action. It is not a productivity dashboard — it's a personal record of the things that matter. The visual system reflects that: composed space, considered color, and type that leads the eye without shouting. Every screen should feel like opening a well-made notebook, not launching a SaaS tool.

The color strategy is **full palette**: 2–3 named pastel roles used deliberately, each with a distinct function. Color carries meaning here — it is not decoration. The overall tone is light and open, with soft surface tints that suggest depth without relying on shadows or blur. Motion is **responsive**: every interaction has a reply, but nothing choreographs itself without user intent.

Jira and Asana are organizational references — their task hierarchy clarity and list legibility inform how TouchGrass structures information. Their visual direction (corporate blues, data-dense grids, infinite sidebar menus) is explicitly rejected. TouchGrass's clarity comes from reduction, not from abundance.

**Key Characteristics:**
- Soft, intentional palette — 2–3 pastel surface roles, not a monochrome neutral field
- Single sans-serif family across all text; clean rendering at all weights required
- Responsive micro-interactions: hover, focus, and state transitions that feel alive but never anxious
- Generous whitespace; content breathes
- Zero decorative elements — no dividers, icons, or patterns without purpose
- Task hierarchy borrowed from Jira/Asana but rendered with Apple-grade restraint

## 2. Colors

A deliberately multi-role pastel palette where each color carries semantic weight. No color is present for decoration alone.

### Primary

- **Soft Sage** (`[to be resolved during implementation]`): The primary action surface — used on primary buttons, active states, and selected items. A desaturated green-grey that reads as calm and forward-moving. Think: checked off, done, moving forward.

### Secondary

- **Warm Blush** (`[to be resolved during implementation]`): The secondary surface role — used on bucket list item backgrounds, category chips, and gentle highlights. Warm but not pink; this is the "this matters to me" color.

### Tertiary

- **Soft Sky** (`[to be resolved during implementation]`): The third accent — used sparingly on progress indicators and interactive states (focus rings, scroll progress). Light blue-grey; the color of possibility.

### Neutral

- **Deep Ink** (`[to be resolved during implementation]`): Primary text. Near-black but not pure black — carries warmth from the palette's hue bias. ≥4.5:1 contrast on all backgrounds.
- **Mid Slate** (`[to be resolved during implementation]`): Secondary text, labels, placeholder text. Must achieve ≥4.5:1 on all background surfaces — test against blush and sage backgrounds specifically.
- **Surface White** (`[to be resolved during implementation]`): The base body background. True off-white at chroma 0 — not warm-tinted paper, not cream. The palette's warmth comes from the accent surfaces, not from tinting the base.
- **Subtle Border** (`[to be resolved during implementation]`): Input strokes, card edges, dividers. Barely-there; present for structure, invisible to the eye.

### Named Rules

**The Three-Voice Rule.** Only Sage (action), Blush (personal/emotional), and Sky (progress/possibility) appear as tinted surfaces. Every other surface is neutral. If a fourth pastel enters the palette, remove one of the three first.

**The Contrast Floor Rule.** Every text element — body, label, placeholder, secondary — must pass WCAG AA (≥4.5:1) at its actual rendered background. Mid Slate on a Blush-tinted card is the failure mode to test first.

## 3. Typography

**Body / Primary Font:** Plus Jakarta Sans (with system sans fallback: -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif)

The choice of Plus Jakarta Sans is deliberate: it has a friendly but not childish geometric construction, a wide weight range (300–800) that enables strong hierarchy from a single family, and excellent rendering at both small and large sizes. No second typeface is needed.

**Character:** Clean and personal. The type stack feels like a well-designed mobile OS — familiar, fast, precise. Weight contrast (300 vs 700) creates hierarchy; size changes support it. No decorative letterforms, no display quirks. The type disappears into content.

### Hierarchy

- **Display** (700, `clamp(1.75rem, 4vw, 2.5rem)`, line-height 1.1, letter-spacing -0.03em): Page-level headings only. Used once per view, if at all.
- **Headline** (600, `clamp(1.25rem, 2.5vw, 1.5rem)`, line-height 1.2): Section and feature headings within a view.
- **Title** (500, 1rem, line-height 1.35): List item titles, card headings, form section labels.
- **Body** (400, 0.9375rem / 15px, line-height 1.6): Primary reading text. Max line length 65–75ch.
- **Label** (500, 0.75rem / 12px, line-height 1.4, letter-spacing 0.01em): Input labels, status chips, metadata. Sentence case only — no all-caps labels.

### Named Rules

**The One Family Rule.** Plus Jakarta Sans only. No secondary typeface, no display script, no serif for headings. Weight contrast achieves hierarchy; a second family introduces noise.

**The Label Case Rule.** Labels are sentence case, never all-caps. Uppercase labels in a soft pastel UI read as shouts.

## 4. Elevation

TouchGrass is flat by default. Depth is expressed through tonal surface differentiation — the three pastel roles (Sage, Blush, Sky) sit above the neutral base as surface layers, not through shadow. This approach is cleaner and more appropriate for a soft, personal app than the classic box-shadow vocabulary.

Shadows appear only in response to state, not as resting decoration. A card at rest is flat; a card that is draggable or being dragged gets a single ambient shadow. Modal overlays get a structural shadow to separate them from the content beneath.

### Shadow Vocabulary

- **Ambient Lift** (`box-shadow: 0 2px 12px rgba(0,0,0,0.06)`): Hover state for interactive cards and list items. Barely perceptible. Confirms interactivity without visual drama.
- **Structural Overlay** (`box-shadow: 0 8px 32px rgba(0,0,0,0.12)`): Dialogs, sheet-style panels. Separates the overlay layer from page content.

### Named Rules

**The Flat-By-Default Rule.** All surfaces are flat at rest. Shadows appear only as feedback for state (hover, drag, overlay). A resting card with a shadow is a design error — it implies interactivity that may not exist.

**The No-Glass Rule.** `backdrop-filter: blur()` is prohibited on all components. Glassmorphism is an explicit anti-reference for this project. If a surface needs to feel "above" another, use the Structural Overlay shadow or tonal layering.

## 5. Components

*This is a seed document; components will be captured in detail on the next `/impeccable document` run once code exists. The entries below are directional.*

### Buttons

Clean, confident geometry. No gradients, no shadows at rest.

- **Shape:** Gently rounded (8px radius)
- **Primary:** Sage background, Deep Ink text, full width on mobile / auto-width on desktop. Padding 12px 24px.
- **Hover:** Sage darkens 8–10%; no transform, no shadow. The color change is the signal.
- **Focus:** 2px offset focus ring in Soft Sky. Visible and clear. No blur.
- **Disabled:** Neutral background (15% opacity of Sage), Mid Slate text. No cursor shimmer.
- **Secondary / Ghost:** Transparent background, Subtle Border stroke, Deep Ink text. Hover: Blush tint at 20% fills the background.

### Inputs / Fields

Tactile and direct. The input is the hero of the auth screens.

- **Style:** White or Surface White background, Subtle Border stroke (1px), 8px radius.
- **Focus:** Border shifts to Sage (2px), no box-shadow glow. Clean and direct.
- **Error:** Border shifts to a soft red-rose (warm, not alarm-red). Error text below field in 12px body.
- **Label:** Sentence-case Label type, Mid Slate color, 8px gap above the input.
- **Placeholder:** Mid Slate at 60% opacity — must still pass 4.5:1 contrast; test before shipping.

### Cards / List Items

The primary content surface for bucket list items.

- **Corner Style:** Gently rounded (10px radius)
- **Background:** Neutral (Surface White) at rest; Blush tint on hover for interactive items
- **Shadow:** None at rest. Ambient Lift on hover (interactive items only).
- **Border:** Subtle Border (1px) at rest. Removed on hover (the Blush tint provides the boundary).
- **Internal Padding:** 16px

### Navigation

Simple, low-chrome. Jira/Asana-clarity without their sidebar complexity.

- **Style:** Top navigation bar, Surface White background, no shadow. Content-width inner container.
- **Active state:** Sage underline (2px) on the active route text. No filled pill, no background fill.
- **Mobile:** Bottom tab bar with 4 primary destinations max. Icon + Thai label, 12px Label type.

## 6. Do's and Don'ts

### Do:
- **Do** use Plus Jakarta Sans at all sizes. Verify rendering across weights — check a multi-weight string in every new component before shipping.
- **Do** keep primary button count to one per view. If two actions compete, the hierarchy is wrong.
- **Do** test Mid Slate text against Blush and Sage surfaces before implementing — this is the most likely contrast failure.
- **Do** use sentence case for all labels, buttons, and navigation items.
- **Do** give interactive elements a clear hover state before a click state. Users shouldn't have to click to discover interactivity.
- **Do** respect `prefers-reduced-motion`: all micro-interactions must have an instant fallback.
- **Do** cap line length at 65–75ch for body text. Wider than that and reading becomes work.
- **Do** borrow task hierarchy clarity from Jira/Asana — legible lists, clear status, scannable structure.

### Don't:
- **Don't** use `backdrop-filter: blur()` on any component. Glassmorphism is an explicit anti-reference.
- **Don't** use decorative gradients. No gradient fills, no `background-clip: text` gradient text, no rainbow-as-identity.
- **Don't** use the indigo/violet SaaS default (#4f46e5 or similar) as the primary brand color. This project moves away from that.
- **Don't** introduce a fourth pastel surface color. Three named roles max (Sage, Blush, Sky); anything beyond that fragments the palette.
- **Don't** use brutalist patterns: heavy borders, raw unstyled type, extreme contrast for shock value.
- **Don't** make the UI feel like AliExpress (cluttered, promotional, noisy) or Wikipedia (unstyled utility, no visual hierarchy, link-blue everywhere).
- **Don't** replicate Jira or Asana's visual language — corporate blue palettes, data-dense metric surfaces, sidebar-heavy navigation. Borrow their information structure, not their aesthetic.
- **Don't** use all-caps labels. Sentence case throughout.
- **Don't** add decorative elements — dividers, icon clusters, section eyebrows — without a specific functional reason.
- **Don't** use box-shadow on resting cards. Shadows appear only as hover/drag/overlay state feedback.
